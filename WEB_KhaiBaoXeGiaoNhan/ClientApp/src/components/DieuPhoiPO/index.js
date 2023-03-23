import { PlusOutlined, WarningOutlined } from "@ant-design/icons";
import {
  Button,
  Card,
  Col,
  Form,
  Input,
  InputNumber,
  message,
  Popconfirm,
  Row,
  Select,
  Space,
  Switch,
  Table,
  Typography,
} from "antd";
import moment from "moment";
import React, { useContext, useEffect, useRef, useState } from "react";
import { GetUserInFo } from "../../Services/AccountSevice/AccountService";
import { GetCungDuongByUser } from "../../Services/CungDuongService/cungDuongService";
import { GetDeliveryServiceList } from "../../Services/DeliveryService/DeliveryService";
import {
  DieuPhoiSave,
  GetDieuPhoiByPoNumber,
} from "../../Services/DieuPhoi/DieuPhoiService";
import AuthContext from "../../store/auth-context";
import HeaderPage from "../Common/HeaderPage";
import TextNumber, { FormatedNumber, WeightParser } from "../Common/TextNumber";
import "./index.less";
const { Text } = Typography;

const { Search } = Input;
const { Option } = Select;

export const DieuPhoiPO = (props) => {
  // useState
  const [lstDVVC, setLstDVVC] = useState([]);
  const [lstCungDuong, setLstCungDuong] = useState([]);
  const [isCont, setIsCont] = useState(false);
  const [selectedPO, setSelectedPO] = useState("");
  const [isSaving, setIsSaving] = useState(false);
  const [detail, setDetail] = useState({});
  const [myCompany, setMyCompany] = useState("");
  const [isLoadingDetail, setIsLoadingDetail] = useState(false);

  const authCtx = useContext(AuthContext);

  const formRef = useRef();

  // Lấy thông tin cá nhân
  useEffect(() => {
    GetUserInFo(authCtx.username).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setMyCompany(objRespone.item.company);
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  }, []);

  // Lấy danh sách cung đường
  useEffect(() => {
    getCungDuong();
  }, []);

  // Lấy thôgn tin theo PO
  useEffect(() => {
    if (setSelectedPO !== "") getDieuPhoiDetail(selectedPO);
  }, [selectedPO]);

  // Lấy ds DVVC

  useEffect(() => {
    GetDeliveryServiceList().then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setLstDVVC(objRespone.data);
      } else {
        setLstDVVC([]);
      }
    });
  }, []);

  const getCungDuong = async () => {
    await GetCungDuongByUser().then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setLstCungDuong(objRespone.data);
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  };

  // lấy thôgn tin điều phối theo PO
  const getDieuPhoiDetail = async (ponumber) => {
    if (ponumber === "") return;
    setIsLoadingDetail(true);

    // xoá dữ liệu cũ khi chọn po mới
    setDetail({});
    formRef.current.setFieldsValue({
      listTransporter: [],
      billNumber: "",
      shipNumber: "",
      isCont: false,
      provider: "",
    });
    await GetDieuPhoiByPoNumber(ponumber).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        console.log(objRespone.item);
        setDetail(objRespone.item);

        if (objRespone.item.mappings) {
          var listResult = objRespone.item.mappings.map((item) => ({
            trongLuongGan: item.isCont ? item.soLuongCont : item.soLuong,
            provider: item.serviceId,
            cungDuong: item.cungDuongCode + "",
          }));

          var billNumber =
            objRespone.item.mappings.length > 0
              ? objRespone.item.mappings[0].billNumber
              : "";
              
          var shipNumber =
            objRespone.item.mappings.length > 0
              ? objRespone.item.mappings[0].shipNumber
              : "";

          formRef.current.setFieldsValue({
            listTransporter: listResult,
            billNumber: billNumber,
            shipNumber: shipNumber,
            provider: objRespone.item.providerId,
            isCont:
              objRespone.item.mappings.length > 0
                ? objRespone.item.mappings[0].isCont
                : false,
          });
        }
      } else {
        message.error(objRespone.err.msgString);
      }
      setIsLoadingDetail(false);
    });
  };

  // handler save
  const onFinish = (values) => {
    // Đổi trạng thái của nút Lưu
    setIsSaving(true);

    // Kiểm tra đã nhập thông tin

    // Update data đúng chuẩn
    console.log(values);
    handleSave(mappingData(values));
  };

  // Lưu thông tin điều phối
  const handleSave = (body) => {
    // Call API Save data
    DieuPhoiSave(body).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        message.success(objRespone.err.msgString);
        // window.location.reload();
      } else {
        message.error(objRespone.err.msgString);
      }
      setIsSaving(false);
    });
  };

  // mapping data truowsc khi lưu
  const mappingData = (valuesForm) => {
    var body = {};

    if (valuesForm.listTransporter.length > 0) {
      body = {
        services: valuesForm.listTransporter.map((transporter) => ({
          servicesID: transporter.provider,
          quantity: valuesForm.isCont ? 0 : transporter.trongLuongGan,
          cungDuongCode: parseInt(transporter.cungDuong),
          soLuongCont: valuesForm.isCont ? transporter.trongLuongGan : 0,
        })),
        masterID: valuesForm.provider,
        orderNumber: selectedPO,
        isCont: valuesForm.isCont,
        billNumber: valuesForm.billNumber,
        shipNumber: valuesForm.shipNumber,
      };
    } else {
      body = {
        services: [],
        masterID: valuesForm.provider,
        orderNumber: valuesForm.orderNumber,
      };
    }
    console.log("aaa", body);

    return body;
  };

  const onReset = () => {
    formRef.current.resetFields();
    setDetail({});
    // this.setState({
    //   polineList: [],
    //   order_selected: {},
    // });
  };

  const totalTrongLuongNhap = () => {
    var list = formRef.current.getFieldValue("listTransporter");
    var totalNhap = 0;
    list.forEach((item) => {
      if (item) totalNhap += item.trongLuongGan || 0;
    });
    return totalNhap;
  };

  // Validate thông tin trọng lượng
  const trongLuongValidator = async (_, value) => {
    if (!value) throw new Error("Chưa nhập trọng lượng");
    if (value <= 0) throw new Error("Chưa nhập trọng lượng");
    var tlChoPhep = detail.tlConLaiChoPhep;
    if (totalTrongLuongNhap() > tlChoPhep)
      throw new Error(
        `Tổng trọng lượng cho phép ${FormatedNumber(tlChoPhep)} kg`
      );
  };

  const poLineColumns = [
    {
      title: "Mục",
      dataIndex: "poline",
      key: "poline",
      width: "10%",
    },
    {
      title: "Mã mặt hàng",
      dataIndex: "productCode",
      width: "17%",
    },
    {
      title: "Tên mặt hàng",
      dataIndex: "productName",
      width: "30%",
    },
    {
      title: "Dơn vị",
      dataIndex: "unit",
      align: "center",
      width: "10%",
    },
    {
      title: "Số lượng",
      align: "right",
      dataIndex: "qty",
      render: (text) => <TextNumber value={text} />,
      width: "12%",
    },
  ];

  // Option select

  // DVVC
  const optionsDVVC = lstDVVC.map((d) => (
    <Option key={d.providerId}>{d.providerName}</Option>
  ));
  // Chọn cung đường
  const optionsCungDuong = lstCungDuong.map((cungDuong) => (
    <Option key={cungDuong.cungDuongCode}>{cungDuong.cungDuongName}</Option>
  ));

  return (
    <React.Fragment>
      <HeaderPage
        title={myCompany}
        description="Khai báo thông tin xe giao/nhận cho đơn hàng"
      />
      <Form
        ref={formRef}
        // onValuesChange={(changedValue, values) => this.checkAllowedSave(changedValue, values)}
        name="register_vehicel_giao_nhan"
        onFinish={onFinish}
        autoComplete="off"
      >
        <Card
          style={{
            width: "100%",
            paddingLeft: "0px",
            border: "0.5px solid #d9d9d9",
            borderRadius: "10px",
          }}
        >
          <Row gutter={24}>
            <Col span={16}>
              <Form.Item
                style={{ marginBottom: "0px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                label="Mã đơn hàng"
                name="orderNumber"
                rules={[{ required: true, message: "Chưa chọn mã đơn hàng" }]}
              >
                <Search
                  // disabled={this.state.disableSelectOrder}
                  // readOnly={true}
                  allowClear
                  // loading={this.state.gettingDetail}
                  onSearch={(po) => setSelectedPO(po)}
                  // onSearch={(po) => getDieuPhoiDetail(po)}
                  style={{ width: "100%" }}
                />

                {/* <Select
                  showSearch
                  // style={props.style}
                  defaultActiveFirstOption={true}
                  // showArrow={false}
                  filterOption={false}
                  // onSearch={(value) => setTextSearchTaiXe(value)}
                  // notFoundContent={
                  //   <Button onClick={() => setIsShowModalCreateTaiXe(true)}>
                  //     Tạo mới
                  //   </Button>
                  // }
                >
                  {optionsOrderNumber}
                </Select> */}
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={24}>
            <Col span={16}>
              <Form.Item
                style={{ marginBottom: "0px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                name="provider"
                label="Nhà cung cấp"
              >
                <Text>
                  {detail.pomaster ? detail.pomaster.providerName : ""}
                </Text>
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={24}>
            <Col span={16}>
              <Form.Item
                style={{ marginBottom: "0px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                name="endDate"
                label="Thời hạn hiệu lực"
              >
                <Text>
                  {detail.polines
                    ? moment(detail.polines[0].deliveryDate).format(
                        "DD/MM/YYYY"
                      )
                    : ""}
                </Text>
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={24}>
            <Col span={16}>
              <Form.Item
                style={{ marginBottom: "0px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                name="total"
                label="Tổng số lượng"
              >
                <Text>
                  {detail.pomaster ? (
                    FormatedNumber(detail.pomaster.qtyTotal)
                  ) : (
                    <>&#8211;</>
                  )}{" "}
                  kg
                </Text>
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={24}>
            <Col span={16}>
              <Form.Item
                style={{ marginBottom: "0px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                name="soLuongDaNhap"
                label="Số lượng đã nhập"
              >
                <Text>
                  {/* <TextNumber value={detail.trongLuongDaNhap} /> kg */}
                </Text>
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={24}>
            <Col span={16}>
              <Form.Item
                style={{ marginBottom: "0px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                name="Note"
                label="Ghi chú"
              >
                <Text>{detail.pomaster ? detail.pomaster.note : ""}</Text>
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={24}>
            <Col span={16}>
              <Form.Item
                style={{ marginBottom: "0px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                name="billNumber"
                label="Nhập số bill"
              >
                <Input />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={24}>
            <Col span={16}>
              <Form.Item
                style={{ marginBottom: "0px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                name="shipNumber"
                label="Tàu vận chuyển"
              >
                <Input />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={24}>
            <Col span={16}>
              <Form.Item
                valuePropName="checked"
                style={{ marginBottom: "0px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                name="isCont"
                label="Điều phối theo Cont"
              >
                <Switch onChange={(checked) => setIsCont(checked)}></Switch>
              </Form.Item>
            </Col>
          </Row>
        </Card>
        <br />
        {/* <Divider orientation="left" >Thông tin mặt hàng</Divider> */}
        <Table
          style={{ border: "0.5px solid #d9d9d9" }}
          locale={{ emptyText: "Chưa có dữ liệu" }}
          columns={poLineColumns}
          dataSource={detail.polines}
          pagination={false}
          // bordered
        />

        {/* </Space> */}
        <br />
        {/* <Divider orientation="left" >Điều phối đơn hàng</Divider> */}

        {/* table-striped */}
        <table className="table" aria-labelledby="tabelLabel">
          <thead
            className="ant-table-thead"
            style={{ border: "0.5px solid #d9d9d9" }}
          >
            <tr>
              <th
                className="ant-table-cell"
                width="45%"
                style={{ textAlign: "center" }}
              >
                Đơn vị vận chuyển
              </th>
              <th className="ant-table-cell" width="32%">
                Cung đường vận chuyển
              </th>
              <th className="ant-table-cell" width="13%">
                {isCont ? "Số cont" : "Trọng lượng (kg)"}
              </th>
              <th
                className="ant-table-cell"
                width="10%"
                style={{ textAlign: "center" }}
              >
                Chức năng
              </th>
            </tr>
          </thead>

          <Form.List name="listTransporter">
            {(fields, { add, remove }) => (
              <>
                <tbody style={{ border: "0.5px solid #d9d9d9" }}>
                  {fields.map((field) => (
                    <tr key={field.key}>
                      <td colSpan={4} style={{ padding: "0px" }}>
                        <table width="100%" className="table-borderless">
                          <tbody>
                            <tr style={{ backgroundColor: "#00000000" }}>
                              <td width="45%">
                                <Form.Item
                                  style={{ marginBottom: "0px" }}
                                  noStyle
                                  shouldUpdate={(prevValues, curValues) =>
                                    prevValues.area !== curValues.area ||
                                    prevValues.sights !== curValues.sights
                                  }
                                >
                                  {() => (
                                    <Form.Item
                                      style={{ marginBottom: "0px" }}
                                      {...field}
                                      name={[field.name, "provider"]}
                                      fieldKey={[field.fieldKey, "provider"]}
                                      rules={[
                                        {
                                          required: true,
                                          message: "Chưa chọn nhà cung cấp",
                                        },
                                      ]}
                                    >
                                      <Select
                                        showSearch
                                        style={props.style}
                                        filterOption={false}
                                        notFoundContent={""}
                                      >
                                        {optionsDVVC}
                                      </Select>
                                    </Form.Item>
                                  )}
                                </Form.Item>
                              </td>
                              <td width="32%">
                                <Form.Item
                                  style={{ marginBottom: "0px" }}
                                  {...field}
                                  name={[field.name, "cungDuong"]}
                                  fieldKey={[field.fieldKey, "cungDuong"]}
                                  rules={[
                                    {
                                      required: true,
                                      message: "Chưa chọn cung đường",
                                    },
                                  ]}
                                >
                                  <Select
                                    // style={{ width: '100%' }}
                                    showSearch
                                    allowClear
                                    // loading={this.state.isFetchingCungDuong}
                                    // placeholder=""
                                    // optionFilterProp="children"
                                    filterOption={(input, option) =>
                                      option.children
                                        .toLowerCase()
                                        .indexOf(input.toLowerCase()) >= 0
                                    }
                                    filterSort={(optionA, optionB) =>
                                      optionA.children
                                        .toLowerCase()
                                        .localeCompare(
                                          optionB.children.toLowerCase()
                                        )
                                    }
                                  >
                                    {optionsCungDuong}
                                  </Select>
                                </Form.Item>
                              </td>
                              <td width="13%">
                                <Form.Item
                                  style={{ marginBottom: "0px" }}
                                  {...field}
                                  name={[field.name, "trongLuongGan"]}
                                  fieldKey={[field.fieldKey, "trongLuongGan"]}
                                  rules={[
                                    {
                                      required: true,
                                      validator: trongLuongValidator,
                                    },
                                  ]}
                                >
                                  <InputNumber
                                    className="my-input-number"
                                    style={{
                                      width: "100%",
                                    }}
                                    formatter={(value) => FormatedNumber(value)}
                                    parser={WeightParser}
                                  />
                                </Form.Item>
                              </td>

                              <td width="10%" style={{ textAlign: "center" }}>
                                <div className="my-popconfirm">
                                  <Popconfirm
                                    title="Bạn có muốn xoá？"
                                    icon={<WarningOutlined />}
                                    okText="Có"
                                    okType="default"
                                    cancelText="Không"
                                    onConfirm={() => remove(field.name)}
                                  >
                                    <Button type="primary" danger>
                                      Xoá
                                    </Button>
                                  </Popconfirm>
                                </div>
                              </td>
                            </tr>
                          </tbody>
                        </table>
                      </td>
                    </tr>
                  ))}
                </tbody>
                <Form.Item style={{ marginTop: "24px", width: "100px" }}>
                  <Button
                    type="dashed"
                    onClick={() => add()}
                    block
                    icon={<PlusOutlined />}
                  >
                    Thêm
                  </Button>
                </Form.Item>
              </>
            )}
          </Form.List>
        </table>
        <Form.Item>
          <Space>
            <Button htmlType="button" onClick={onReset}>
              Huỷ
            </Button>
            <Button
              type="primary"
              disabled={isLoadingDetail}
              htmlType="submit"
              loading={isSaving}
            >
              Lưu
            </Button>
          </Space>
        </Form.Item>
      </Form>

      <i>
        Chú thích: <>&#91;</>
        <a style={{ color: "red" }}>*</a>
        <>&#93;</> là thông tin bắt buộc.
      </i>
    </React.Fragment>
  );
};

export default DieuPhoiPO;
