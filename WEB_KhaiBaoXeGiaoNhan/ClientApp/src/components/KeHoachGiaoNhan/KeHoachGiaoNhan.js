import { ExclamationCircleOutlined, WarningOutlined } from "@ant-design/icons";
import {
  Button,
  DatePicker,
  Divider,
  Form,
  Input,
  InputNumber,
  message,
  Modal,
  Select,
  Space,
  Table,
  Tooltip,
  Typography,
} from "antd";
import locale from "antd/es/date-picker/locale/vi_VN";
import moment from "moment";
import "moment/locale/vi";
import React, { Component } from "react";
import { myCheckAuth } from "../../Services/authServices";
import { GetListCompany } from "../../Services/CompanyService";
import { SearchDriver } from "../../Services/DriverService/driverService";
import {
  ActiveVehicleRegister,
  GetVehicleRegisterList2Edit,
  RemoveMultipleVehicleRegister,
  RemoveVehicleRegister,
  UpdateVehicleRegister,
} from "../../Services/KhaiBaoService/khaiBaoService";
import { GetActiveOrderList } from "../../Services/OrderServices/orderService";
import { WeightParser } from "../Common/TextNumber";
import "./KeHoachGiaoNhan.less";

const { RangePicker } = DatePicker;

const { confirm } = Modal;
const { Text } = Typography;
const { Option } = Select;
const layout = {
  labelCol: { span: 9 },
  wrapperCol: { span: 15 },
};

export class KeHoachGiaoNhan extends Component {
  static displayName = KeHoachGiaoNhan.name;
  formRef = React.createRef();

  // Check login
  componentWillMount() {
    myCheckAuth();
  }
  constructor(props) {
    super(props);
    this.state = {
      listVehicleRegister: [],
      loading: true,
      visibleCreateDriver: false,
      visibleUpdateRecord: false,
      taiXeList: [],
      selectedRecord: {},
      driverSelected: "",
      vatTuList: [],
      updating: false,
      orderList: [],
      loadingOrder: false,
      plants: [],
      selectedPlant: undefined,
      selectedRowKeys: [],
      deleting: false
    };
    this.handleCloseModal = this.handleCloseModal.bind(this);
    this.showConfirmMultipleDelete = this.showConfirmMultipleDelete.bind(this);
    this.showUpdateVehicleRegister = this.showUpdateVehicleRegister.bind(this);
  }

  componentDidMount() {
    this.getNoiGioaNhan();
    this.getListVehicleRegister();
    this.getPOListActive();
  }

  // hiện Modal tạo mới driver
  showCreateDriver() {
    // this.setState({
    //   visibleCreateDriver: true,
    // });
  }
  onCreateDriverHandler = (value) => {
    console.log(value);
  };

  onSelectChange = selectedRowKeys => {
    console.log('selectedRowKeys changed: ', selectedRowKeys);
    this.setState({ selectedRowKeys });
  };

  showUpdateVehicleRegister(record) {
    // console.log(record)
    var that = this;
    var currentTaiXe = {
      driverId: record.driver.driverId,
      driverName: record.driverName,
      driverCardNo: record.driverIdCard,
    };
    this.setState(
      {
        visibleUpdateRecord: true,
        selectedRecord: record,
        vatTuList: record.polines,
        taiXeList: [currentTaiXe],
      },
      () => {
        // console.log('success setState');
        // set timeout để đảm bảo form đã được render xong rồi mới set giá trị cho form

        setTimeout(function () {
          try {
            that.formRef.current.setFieldsValue({
              driverid: record.driver.driverId,
              vatTuUpdate: record.vatTu.map((item) => item.productCode),
            });
          } catch (ex) {
            console.log(ex);
          }
        }, 1000); // 1s
      }
    );
    // console.log([record.vatTu.map(item => item.productCode)]);
    // console.log(this.state.selectedRecord)
  }

  // SHow xác nhận xoá xe
  showConfirmDelete(record) {
    var that = this;
    confirm({
      title: (
        <p>
          Xóa thông tin giao/nhận của xe{" "}
          <Text strong> {record.vehicleNumber}</Text>
        </p>
      ),
      icon: <WarningOutlined />,
      content: "",
      // okType: 'default',
      okText: "Huỷ",
      cancelText: "Xoá",
      keyboard: false,
      onOk() {
        console.log("Cancel");
      },
      onCancel() {
        // Gọi APi xác nhận xoá Xe
        RemoveVehicleRegister(record.key).then((objRespone) => {
          if (objRespone.isSuccess === true) {
            message.success(objRespone.err.msgString);
            that.getListVehicleRegister(
              undefined,
              undefined,
              undefined,
              that.state.selectedPlant
            );
          } else {
            message.error(objRespone.err.msgString);
          }
        });
      },
    });
  }

  // SHow xác nhận xoá xe
  showConfirmMultipleDelete() {
    var that = this;
    confirm({
      title: (
        <p>
          Xóa thông tin giao/nhận của {this.state.selectedRowKeys.length} xe đã chọn
        </p>
      ),
      icon: <WarningOutlined />,
      content: "",
      // okType: 'default',
      okText: "Huỷ",
      cancelText: "Xoá",
      keyboard: false,
      onOk() {
        console.log("Cancel");
      },
      onCancel() {
        that.setState({
          deleting: true
        })
        // Gọi APi xác nhận xoá Xe
        RemoveMultipleVehicleRegister(that.state.selectedRowKeys).then((objRespone) => {
          if (objRespone.isSuccess === true) {
            message.success(objRespone.err.msgString);
            that.getListVehicleRegister(
              undefined,
              undefined,
              undefined,
              that.state.selectedPlant
            );
            
          } else {
            message.error(objRespone.err.msgString);
            
          }
          that.setState({
            deleting: false
          })
        });
      },
    });
  }

  // Xác nhậc Active xe
  showConfirmActive(record) {
    var that = this;
    confirm({
      title: (
        <p>
          Kích hoạt xe <Text strong> {record.vehicleNumber}</Text> vào cổng{" "}
        </p>
      ),
      icon: <WarningOutlined />,
      content: "",
      // okType: 'default',
      okText: "Huỷ",
      cancelText: "Kích hoạt",
      keyboard: false,
      onOk() {
        console.log("Cancel");
      },
      onCancel() {
        // Gọi APi cập nhật thông tin để active xe
        console.log(record);
        that.setState({ updating: true });

        ActiveVehicleRegister(record.key).then((objRespone) => {
          if (objRespone.isSuccess === true) {
            message.success(objRespone.err.msgString);
            that.handleCloseModal();
            that.setState({ loading: true, updating: false });
            that.getListVehicleRegister(
              undefined,
              undefined,
              undefined,
              that.state.selectedPlant
            );
          } else {
            message.error(objRespone.err.msgString);
            that.setState({ loading: false, updating: false });
          }
        });
      },
    });
  }

  // update lại danh sách vật tư khi thay đổi po
  handleChangeOrder(order) {
    // console.log(order)
    this.formRef.current.setFieldsValue({
      vatTuUpdate: [],
    });
    this.setState({
      vatTuList: order.vattu,
    });
  }

  handleCloseModal() {
    this.setState({
      visibleCreateDriver: false,
      visibleUpdateRecord: false,
    });
  }

  // Cập nhật thông tin đăng ký
  updateVehicleRegister(values, key) {
    this.setState({ updating: true });
    var body = {
      orderNumber: values.soDonHang,
      driverid: values.driverid,
      trongLuong:
        values.trongLuongGiaoDuKien == null ? 0 : values.trongLuongGiaoDuKien,
      vatTuUpdate: values.vatTuUpdate,
      assets: values.assets,
    };
    // console.log(body)

    UpdateVehicleRegister(key, body).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        message.success(objRespone.err.msgString);
        this.handleCloseModal();
        this.setState({ loading: true, updating: false });
        this.getListVehicleRegister(
          undefined,
          undefined,
          undefined,
          this.state.selectedPlant
        );
      } else {
        message.error(objRespone.err.msgString);
        this.setState({ loading: false, updating: false });
      }
    });
  }

  render() {
    const { selectedRowKeys } = this.state;
    const columns = [
      {
        title: "Ngày giao nhận",
        dataIndex: "thoiGianToiDuKien",
        key: "thoiGianToiDuKien",
        // width: '12.5%',
        // fixed: 'left',
        render: (text, row) => row.thoiGianToiDuKien.format("DD/MM/YYYY"),
      },
      {
        title: "Mã đơn hàng",
        dataIndex: "soDonHang",
        key: "soDonHang",
        // width: '11%',
      },
      {
        title: "Biển số xe",
        dataIndex: "vehicleNumber",
        key: "vehicleNumber",
        render: (text, row) =>
          text + (row.romooc !== null ? " + " + row.romooc : ""),
        // width: '10%',
      },
      // {
      //   title: "Rơ mooc",
      //   dataIndex: "romooc",
      //   key: "romooc",
      //   // width: '10%',
      // },

      {
        title: "Tài xế",
        dataIndex: "driverName",
        key: "driverName",
        render: (text, row) => (
          <p style={{ display: "inline" }}>
            {text}
            <br />
            {row.driverIdCard}
          </p>
        ),
        // width: '26%',
      },

      {
        title: "Mặt hàng",
        dataIndex: "vatTu",
        key: "vatTu",
        render: (text, row) =>
          row.vatTu.map((item) => (
            <p style={{ display: "inline" }} key={item.productCode}>
              {item.productName}
              <br />
            </p>
          )),
      },
      {
        title: "Trọng lượng",
        dataIndex: "trongLuongGiaoDuKien",
        key: "trongLuongGiaoDuKien",
        render: (text, row) => (
          <>
            {new Intl.NumberFormat("de-DE").format(text) == 0 ? (
              <>&#8210;</>
            ) : (
              new Intl.NumberFormat("de-DE").format(text)
            )}
            {row.isQuaTai ? (
              <Tooltip
                title={
                  <>
                    Quá tải
                    <br />
                    Trọng lượng cho phép:{" "}
                    {new Intl.NumberFormat("de-DE").format(
                      row.weightAllowed
                    )}{" "}
                    kg
                  </>
                }
              >
                {" "}
                <ExclamationCircleOutlined style={{ color: "orange" }} />
              </Tooltip>
            ) : (
              ""
            )}
          </>
        ),
        align: "right",
        // width: '10.5%'
      },

      {
        title: "Thay đổi",
        dataIndex: "action",
        key: "action",
        align: "right",
        render: (text, record) => (
          <>
            {record.allowEdit ? (
              <Space size="middle">
                {!record.isActive ? (
                  <a
                    key="active"
                    onClick={() => {
                      this.showConfirmActive(record);
                    }}
                  >
                    <Button size="small" style={{ borderColor: "#28a745" }}>
                      Kích hoạt
                    </Button>
                  </a>
                ) : (
                  <></>
                )}

                <a
                  key="edit"
                  onClick={() => {
                    this.showUpdateVehicleRegister(record);
                  }}
                >
                  <Button size="small">Sửa</Button>
                </a>

                <a
                  key="delete"
                  onClick={() => {
                    this.showConfirmDelete(record);
                  }}
                >
                  <Button size="small" danger>
                    Xoá
                  </Button>
                </a>
              </Space>
            ) : (
              <Space></Space>
            )}
          </>
        ),
        // width: '10%',
      },
    ];

    const optionsTaiXe = this.state.taiXeList.map((d) => (
      <Option key={d.driverId}>
        {d.driverName} - {d.driverCardNo}
      </Option>
    ));
    const optionsVatTu = this.state.vatTuList.map((d) => (
      <Option key={d.productCode}>{d.productName}</Option>
    ));
    const optionsOrder = this.state.orderList.map((d) => (
      <Option key={d.orderNumber} vattu={d.vattu}>
        {d.orderNumber}
      </Option>
    ));

    const optionsPlant = this.state.plants.map((plant) => (
      <Option key={plant.companyCode} value={plant.companyCode}>
        {plant.companyName}
      </Option>
    ));

    

    const rowSelection = {
      selectedRowKeys,
      onChange: this.onSelectChange,
    };
    const hasSelected = selectedRowKeys.length > 0;

    return (
      <div>
        <h2 id="tabelLabel">Kế hoạch giao nhận</h2>
        <p>Danh sách các xe đã được khai báo và đang chờ vận chuyển</p>
        <br />
        <br />
        {/* <h4>Kế hoạch giao nhận</h4> */}

        <div>
          <div align="baseline" style={{ width: "100%" }}>
            <Space>
              <Form
                onFinish={(value) => {
                  this.searchListVehicleRegister(value);
                }}
                layout="inline"
              >
                <Form.Item
                  name="time"
                  initialValue={[moment(), moment()]}
                  rules={[{ required: true, message: "Chưa chọn thời gian" }]}
                >
                  <RangePicker
                    locale={locale}
                    format="DD/MM/YYYY"
                    placeholder={["Từ ngày", "Đến ngày"]}
                  />
                </Form.Item>
                <Form.Item
                  name="orderNumber"
                  // rules={[
                  //   { required: true, message: 'Chưa nhập thônng tin' },
                  // ]}
                >
                  <Input placeholder="Mã đơn hàng"></Input>
                </Form.Item>
                <Form.Item
                  name="plant"
                  // rules={[
                  //   { required: true, message: 'Chưa nhập thônng tin' },
                  // ]}
                >
                  <Select
                    style={{ minWidth: 240 }}
                    placeholder="Chọn nơi giao nhận"
                    allowClear
                  >
                    {optionsPlant}
                  </Select>
                </Form.Item>
                <Form.Item>
                  <Button type="primary" htmlType="submit">
                    Tìm kiếm
                  </Button>
                </Form.Item>
              </Form>
            </Space>
            <Text className="text-trong-luong">Trọng lượng, kg</Text>
          </div>

          <Divider />
        </div>

        <div className="d-flex justify-content-end" style={{paddingBottom: "14px"}} >
        <Button type="primary" onClick={this.showConfirmMultipleDelete} disabled={!hasSelected} loading={this.state.deleting}>
            Xóa mục đã chọn
        </Button>
        </div>


        <Table
          rowSelection={rowSelection} 
          columns={columns}
          dataSource={this.state.listVehicleRegister}
          loading={this.state.loading}
          style={{ width: "100%" }}
          locale={{ emptyText: "Không có dữ liệu" }}
          pagination={{ showSizeChanger: true }}
          // scroll={{ x: 1300 }}
          // expandable={{
          //   expandedRowRender: record => <Descriptions bordered>
          //     <Descriptions.Item label="Vật tư"><>{record.vatTu.map(item => (<p>{item.productName}</p>))}</></Descriptions.Item>
          //     <Descriptions.Item label="Cung đường">{record.cungDuongName}</Descriptions.Item>
          //     <Descriptions.Item label="Tài sản theo xe">{record.assets}</Descriptions.Item>
          //   </Descriptions>,
          // }}
        />

        {/* Hiện form cập nhật thông tin xe đăng ký */}

        {/* <UpdateRegister
          visible={this.state.visibleUpdateRecord}
          onCancel={this.handleCloseModal}
          selectedRecord={this.state.selectedRecord}
          saving={this.state.updating}
        /> */}

        <Modal
          title="Cập nhật thông tin"
          visible={this.state.visibleUpdateRecord}
          onCancel={this.handleCloseModal}
          destroyOnClose={true}
          footer={[
            <Button
              type="primary"
              form="formUpdate"
              key="submit"
              htmlType="submit"
              loading={this.state.updating}
            >
              Cập nhật
            </Button>,
          ]}
        >
          <Form
            ref={this.formRef}
            id="formUpdate"
            initialValues={this.state.selectedRecord}
            {...layout}
            onFinish={(values) =>
              this.updateVehicleRegister(values, this.state.selectedRecord.key)
            }
          >
            <Form.Item
              labelAlign="left"
              name="thoiGianToiDuKien"
              label="Ngày vận chuyển"
            >
              <DatePicker
                format="DD/MM/YYYY"
                disabled
                style={{ width: "100%" }}
              />
            </Form.Item>

            <Form.Item
              labelAlign="left"
              name="soDonHang"
              label="Mã đơn hàng"
              rules={[
                {
                  required: true,
                  message: "Chưa chọn đơn hàng",
                },
              ]}
            >
              <Select
                style={this.props.style}
                // showArrow={false}
                // disabled={this.state.loading}
                disabled
                filterOption={false}
                notFoundContent={<>Không có đơn hàng</>}
                onChange={(value, option) => this.handleChangeOrder(option)}
              >
                {optionsOrder}
              </Select>
            </Form.Item>

            <Form.Item
              labelAlign="left"
              name="vehicleNumber"
              label="Biển số xe"
            >
              <Input disabled />
            </Form.Item>
            <Form.Item labelAlign="left" name="romooc" label="Rơ mooc">
              <Input disabled />
            </Form.Item>

            <Form.Item
              labelAlign="left"
              name="driverid"
              label="Tài xế"
              rules={[{ required: true, message: "Chưa chọn tài xế" }]}
            >
              <Select
                showSearch
                style={this.props.style}
                showArrow={false}
                filterOption={false}
                onSearch={(value) => {
                  this.handleSearchTaiXe(value);
                }}
                notFoundContent={
                  <Button onClick={this.showCreateDriver}>Tạo mới</Button>
                }
              >
                {optionsTaiXe}
              </Select>
              {/* <SelectTaiXe /> */}
            </Form.Item>

            <Form.Item
              labelAlign="left"
              name="vatTuUpdate"
              label="Mặt hàng"
              rules={[{ required: true, message: "Chưa chọn mặt hàng" }]}
            >
              <Select
                mode="multiple"
                allowClear
                // placeholder="Please select"
                // defaultValue={}
                // onChange={handleChange}
              >
                {optionsVatTu}
              </Select>
            </Form.Item>

            <Form.Item
              labelAlign="left"
              name="trongLuongGiaoDuKien"
              label="Trọng lượng hàng (kg)"
              rules={[
                {
                  type: "number",
                  min: 0,
                  max: 99999,
                  message: "Vui lòng nhập đúng giá trị",
                },
              ]}
            >
              <InputNumber
                className="my-input-number"
                style={{ width: "100%", textAlign: "right" }}
                formatter={(value) =>
                  new Intl.NumberFormat("de-DE").format(value)
                }
                parser={WeightParser}
              />
            </Form.Item>
            {/* <Form.Item labelAlign='left' name="assets" label='Tài sản theo xe'>
              <TextArea />
            </Form.Item> */}
          </Form>
        </Modal>

        {/* Tạo mới tài xế */}

        {/* <CreateDriver
          visible={this.state.visibleCreateDriver}
          onCancel={this.handleCloseModal}
          onFinish={this.onCreateDriverHandler}
        /> */}

        {/* <Modal
          visible={this.state.visibleCreateDriver}
          title="Thêm tài xế"
          onCancel={this.handleCloseModal}
          footer={[
            <Button
              type="primary"
              form="formAdd"
              key="submit"
              htmlType="submit"
            >
              Lưu
            </Button>,
          ]}
        >
          <Form
            id="formAdd"
            onFinish={(values) => {
              this.addDriver(values);
            }}
          >
            <Form.Item
              name="driverName"
              label="Tên tài xế"
              rules={[
                {
                  required: true,
                  message: "Chưa nhập tên tài xế!",
                },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              name="driverCardNo"
              label="CMND/GPLX"
              rules={[
                {
                  required: true,
                  message: "Chưa nhập thông tin!",
                },
              ]}
            >
              <Input />
            </Form.Item>
          </Form>
        </Modal> */}
      </div>
    );
  }

  searchListVehicleRegister(values) {
    // console.log(values);
    var startDate = values.time[0].format("YYYY-MM-DD");
    var endDate = values.time[1].format("YYYY-MM-DD");
    this.setState({
      selectedPlant: values.plant,
    });

    // message.loading('Đang tìm kiếm')

    this.getListVehicleRegister(
      startDate,
      endDate,
      values.orderNumber,
      values.plant
    );
  }

  // Tìm kiếm Tên tài xế
  async handleSearchTaiXe(text) {
    if (text.length >= 3) {
      await SearchDriver(text).then((objRespone) => {
        if (objRespone.isSuccess === true) {
          this.setState({
            taiXeList: objRespone.data,
          });
        } else {
          this.setState({
            taiXeList: [],
          });
        }
      });
    } else {
      this.setState({
        taiXeList: [],
      });
    }
  }

  getPOActive(objRespone, isService) {
    var orders = [];
    // if (isService === 'true') {
    console.log("check" + isService);
    for (var i = 0; i < objRespone.item.poResponses.length; i++) {
      orders.push({
        key: objRespone.item.poResponses[i].pomasters.ponumber,
        provider: objRespone.item.poResponses[i].pomasters.providerName,
        orderNumber: objRespone.item.poResponses[i].pomasters.ponumber,
        total: objRespone.item.poResponses[i].pomasters.qtyTotal,
        vattu: objRespone.item.poResponses[i].polines,
        deliveryDate: objRespone.item.poResponses[i].polines[0].deliveryDate,
        soLuongDaChuyen: objRespone.item.poResponses[i].pomasters.soLuongDaNhap,
        // conLai: objRespone.item.poResponses[i].pomasters.qtyTotal - objRespone.item.poResponses[i].pomasters.soLuongDaNhap,
        conLai:
          objRespone.item.poResponses[i].pomasters.qtyTotal -
          objRespone.item.poResponses[i].registered -
          objRespone.item.poResponses[i].trongLuongDaNhap,
        registered:
          objRespone.item.poResponses[i].registered +
          objRespone.item.poResponses[i].trongLuongDaNhap,
      });
    }
    return orders;
    // }
    // if (objRespone.item.poResponses[0]) {
    //   orders.push({
    //     key: objRespone.item.poResponses[0].pomasters.ponumber,
    //     provider: objRespone.item.poResponses[0].pomasters.providerName,
    //     orderNumber: objRespone.item.poResponses[0].pomasters.ponumber,
    //     total: objRespone.item.poResponses[0].pomasters.qtyTotal,
    //     vattu: objRespone.item.poResponses[0].polines,
    //     deliveryDate: objRespone.item.poResponses[0].polines[0].deliveryDate,
    //     soLuongDaChuyen: objRespone.item.poResponses[0].pomasters.soLuongDaNhap,
    //     // conLai: objRespone.item.poResponses[i].pomasters.qtyTotal - objRespone.item.poResponses[i].pomasters.soLuongDaNhap,
    //     conLai: objRespone.item.poResponses[0].pomasters.qtyTotal - objRespone.item.poResponses[0].registered,
    //     registered: objRespone.item.poResponses[0].registered,
    //   })
    // }
    // return orders;
  }

  async getNoiGioaNhan() {
    GetListCompany().then((objRespone) => {
      if (objRespone.isSuccess === true) {
        this.setState({
          plants: objRespone.data,
        });
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  }

  // Lấy danh sách po đang active
  async getPOListActive() {
    this.setState({
      loadingOrder: true,
    });
    var isService = localStorage.getItem("isService") || false;
    await GetActiveOrderList().then((objRespone) => {
      if (objRespone.isSuccess === true) {
        var order = this.getPOActive(objRespone, isService);
        // for (var i = 0; i < objRespone.item.poResponses.length; i++) {
        //   order.push({
        //     key: objRespone.item.poResponses[i].pomasters.ponumber,
        //     provider: objRespone.item.poResponses[i].pomasters.providerName,
        //     orderNumber: objRespone.item.poResponses[i].pomasters.ponumber,
        //     total: objRespone.item.poResponses[i].pomasters.qtyTotal,
        //     vattu: objRespone.item.poResponses[i].polines,
        //     deliveryDate: objRespone.item.poResponses[i].polines[0].deliveryDate,
        //     soLuongDaChuyen: objRespone.item.poResponses[i].pomasters.soLuongDaNhap,
        //     // conLai: objRespone.item.poResponses[i].pomasters.qtyTotal - objRespone.item.poResponses[i].pomasters.soLuongDaNhap,
        //     conLai: objRespone.item.poResponses[i].pomasters.qtyTotal - objRespone.item.poResponses[i].registered,
        //     registered: objRespone.item.poResponses[i].registered,
        //   })
        // }

        this.setState({
          orderList: order,
        });
      } else {
        message.error(objRespone.err.msgString);
      }
      this.setState({ loadingOrder: false });
    });
  }

  async getListVehicleRegister(startDate, endDate, orderNumber, plant) {
    this.setState({
      selectedRowKeys : [],
      loading: true,
    });
    if (localStorage.getItem("type") === "Customer") {
    } else {
      if (startDate == undefined) {
        startDate = moment().format("YYYY-MM-DD");
        endDate = moment().add(1, "month").format("YYYY-MM-DD");
      }
      await GetVehicleRegisterList2Edit(
        startDate,
        endDate,
        orderNumber,
        plant,
        false
      ).then((objRespone) => {
        if (objRespone.isSuccess === true) {
          this.setState({
            loading: false,
            listVehicleRegister: objRespone.data.map((record) => {
              record.key = record.item.vehicleRegisterMobileId;
              record.vehicleNumber = record.item.vehicleNumber;
              record.soDonHang = record.item.soDonHang;
              record.driverName = record.item.driverName;
              record.driverIdCard = record.item.driverIdCard;
              record.trongLuongGiaoDuKien = record.item.trongLuongGiaoDuKien;
              record.trongLuongGiaoThucTe = record.item.trongLuongGiaoThucTe;
              record.thoiGianToiThucTe = record.item.thoiGianToiThucTe;
              record.allowEdit = record.item.allowEdit;
              record.isActive = record.item.isActive;
              record.cungDuongCode = record.item.cungDuongCode;
              record.cungDuongName = record.item.cungDuongName;
              record.vatTu = record.detail.map((detail) => ({
                productCode: detail.productCode,
                productName: detail.productName,
              }));
              record.taiSanTheoXe = record.item.note;
              record.assets = record.item.assets;
              record.thoiGianToiDuKien = moment(record.item.thoiGianToiDuKien);
              record.romooc = record.item.romooc;
              return record;
            }),
          });
        } else {
          this.setState({
            loading: false,
          });
          message.error(objRespone.err.msgString);
        }
      });
    }
  }

  // async populateWeatherData() {
  //   const response = await fetch('weatherforecast');
  //   const data = await response.json();
  //   this.setState({ forecasts: data, loading: false });
  // }
}
