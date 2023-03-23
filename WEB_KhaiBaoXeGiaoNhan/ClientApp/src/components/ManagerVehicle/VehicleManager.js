import { WarningOutlined } from "@ant-design/icons";
import {
  Button,
  Divider,
  Form,
  Input,
  message,
  Modal,
  Select,
  Space,
  Table,
  Typography,
} from "antd";
import React, { Component } from "react";
import { myCheckAuth } from "../../Services/authServices";
import {
  GetVehicleList,
  RemoveVehicle,
  SearchVehicleModel,
} from "../../Services/VehicleService/vehicleService";
import CreateVehicle from "../Modals/CreateVehicle";
import ModalUpdateVehicle from "../Modals/ModalUpdateVehicle";

const { Text } = Typography;
const { Option } = Select;
const { Column } = Table;
const { confirm } = Modal;

export class VehicleManager extends Component {
  static displayName = VehicleManager.name;

  constructor(props) {
    super(props);
    this.state = {
      // vendorList: [],
      vehicleList: [],
      visibleCreateVehicle: false,
      visibleUpadte: false,
      // vehicleType: 1,
      taiXeList: [],
      romoocList: [],
      selectedVehicle: {},
    };
    this.showModalAddVehicle = this.showModalAddVehicle.bind(this);
    this.showUpdateVehicle = this.showUpdateVehicle.bind(this);
    this.handleCloseModal = this.handleCloseModal.bind(this);
    this.getVehicleList = this.getVehicleList.bind(this);
    this.handleSuccessCreateVehicle = this.handleSuccessCreateVehicle.bind(
      this
    );
  }
  // Check login
  componentWillMount() {
    myCheckAuth();
  }

  componentDidMount() {
    this.getVehicleList(1000, 1);
  }

  // Xự kiện tìm kiếm nhà cung cấp
  // handleSearch = value => {
  //   if (value.length >= 3) {
  //     GetVendorList(value).then((objRespone) => {
  //       if (objRespone.isSuccess === true) {
  //         this.setState({
  //           loading: false,
  //           vendorList: objRespone.data
  //         })

  //       } else {
  //         message.error(objRespone.err.msgString)
  //       }
  //     })
  //     // fetch(value, data => this.setState({ data }));
  //   } else {
  //     this.setState({ data: [] });
  //   }
  // };

  handleChange = (value) => {
    this.setState({ value });
  };

  // Lấy danh sách Xe
  getVehicleList(pageSize, pageNumber) {
    GetVehicleList(pageSize, pageNumber).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        this.setState({
          loading: false,
          vehicleList: objRespone.data,
        });
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  }

  handleCloseModal() {
    this.setState({
      visibleCreateVehicle: false,
      visibleUpadte: false,
    });
  }

  handleSuccessCreateVehicle() {
    this.getVehicleList(1000, 1);
    this.handleCloseModal();
  }

  showModalAddVehicle() {
    this.setState({
      visibleCreateVehicle: true,
    });
  }

  searchVehicle(text) {
    // Goị API tìm kiếm Xe theo bsx
    message.loading("Đang tìm kiếm");
    console.log(text);
    SearchVehicleModel(text.search).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        this.setState({
          loading: false,
          vehicleList: objRespone.data,
        });
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  }

  showUpdateVehicle(record) {
    console.log(record);

    this.setState({ selectedVehicle: record, visibleUpadte: true });
    if (record.driverId !== null) {
      var taiXe = {
        driverId: record.driverId,
        driverName: record.driverName,
        driverCardNo: record.driverCardNo,
      };
      this.setState({
        taiXeList: [taiXe],
      });
    }

    // Set Romooc

    if (record.romoocId !== null) {
      var romooc = {
        vehicleId: record.romoocId,
        vehicleNumber: record.romoocNumber,
      };
      this.setState({
        romoocList: [romooc],
      });
    }
  }

  // SHow xác nhận xoá xe
  showConfirmDelete(record) {
    var that = this;
    confirm({
      title: (
        <p>
          Xác nhận xoá xe <Text strong> {record.vehicleNumber}</Text>
        </p>
      ),
      icon: <WarningOutlined />,
      content: "",
      onOk() {},
      okText: "Huỷ",
      // okType: 'default',
      cancelText: "Xoá",
      keyboard: false,
      onCancel() {
        // Gọi APi xác nhận xoá Xe
        RemoveVehicle(record.vehicleId).then((objRespone) => {
          if (objRespone.isSuccess === true) {
            message.success(objRespone.err.msgString);
            that.getVehicleList(1000, 1);
          } else {
            message.error(objRespone.err.msgString);
          }
        });
      },
    });
  }

  render() {
    return (
      <div>
        <h2>Danh sách xe</h2>
        <p>Quản lý danh sách phương tiện cho việc vận chuyển hàng</p>
        <br />
        <br />

        {/* <button className="btn btn-primary" onClick={this.showModalAddDriver}>Thêm tài xế</button> */}
        <Space align="baseline">
          <Form
            onFinish={(value) => {
              this.searchVehicle(value);
            }}
            layout="inline"
          >
            <Form.Item
              name="search"
              rules={[
                { required: true, message: "Chưa nhập BSX" },
                { min: 3, message: "Nhập ít nhất 3 lý tự" },
              ]}
            >
              <Input placeholder="Nhập BSX" />
            </Form.Item>
            <Form.Item>
              <Button type="primary" htmlType="submit">
                Tìm kiếm
              </Button>
            </Form.Item>
          </Form>
          <Button type="primary" onClick={this.showModalAddVehicle}>
            Thêm xe mới
          </Button>
        </Space>
        <Divider />
        <Table
          dataSource={this.state.vehicleList}
          locale={{ emptyText: "Chưa có dữ liệu" }}
        >
          <Column
            title="Biển số xe"
            dataIndex="vehicleNumber"
            key="vehicleNumber"
            render={(bsx) => <Text strong>{bsx}</Text>}
            // width="13%"
          />
          <Column
            // width="30%"
            align="center"
            title="Loại xe"
            dataIndex="isDauKeo"
            key="isDauKeo"
            render={(isDauKeo, row) => (
              <>
                {isDauKeo ? (
                  <Text>Đầu kéo</Text>
                ) : row.isRoMooc === 1 ? (
                  <Text>Ro Mooc</Text>
                ) : (
                  <Text>Xe thường</Text>
                )}
              </>
            )}
            // width="13%"
          />
          <Column
            align="right"
            title="Trọng lượng bì, kg"
            dataIndex="vehicleWeight"
            key="vehicleWeight"
            render={(value) => new Intl.NumberFormat("de-DE").format(value)}
            // width="17%"
          />

          <Column
            align="right"
            title={<>Trọng lượng toàn tải TGGT, kg</>}
            dataIndex="trongLuongDangKiem"
            key="trongLuongDangKiem"
            render={(value) => new Intl.NumberFormat("de-DE").format(value)}
            // width="19%"
          />
          <Column align="center" title="Romooc" dataIndex="romoocNumber" />
          <Column align="left" title="Tài xế" dataIndex="driverName" />
          <Column
            title="Thay đổi"
            // width="10%"
            key="action"
            align="center"
            render={(text, record) => (
              <Space size="middle">
                <a
                  key="edit"
                  onClick={() => {
                    this.showUpdateVehicle(record);
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
            )}
          />
        </Table>

        {/* Tạo mới Xe */}
        <CreateVehicle
          createModalVisible={this.state.visibleCreateVehicle}
          onCancel={this.handleCloseModal}
          onSuccess={this.handleSuccessCreateVehicle}
        />

        {/* Cập nhật thông tin xe */}
        <ModalUpdateVehicle
          visible={this.state.visibleUpadte}
          onCancel={this.handleCloseModal}
          onSuccess={this.handleSuccessCreateVehicle}
          initialValues={this.state.selectedVehicle}
          listTaiXe={this.state.taiXeList}
          listRomooc={this.state.romoocList}
        />
      </div>
    );
  }
}
