import React, { Component } from "react";
import {
  Form,
  Input,
  Button,
  Popconfirm,
  Select,
  DatePicker,
  Table,
  message,
  InputNumber,
  Modal,
  Col,
  Row,
  Tooltip,
  Card,
  Spin,
  Checkbox,
} from "antd";
import {
  PlusOutlined,
  WarningOutlined,
  CloseOutlined,
} from "@ant-design/icons";
import { myCheckAuth } from "../../Services/authServices";
import { GetActiveOrderList } from "../../Services/OrderServices/orderService";
// import { GetCungDuongList } from '../../Services/CungDuongService/cungDuongService';
import {
  AddVehicle,
  SearchBSX,
} from "../../Services/VehicleService/vehicleService";
import {
  AddDriver,
  SearchDriver,
} from "../../Services/DriverService/driverService";
import moment from "moment";
import "moment/locale/vi";
import "./index.less";
import locale from "antd/es/date-picker/locale/vi_VN";
import CreateVehicle from "../Modals/CreateVehicle";
import DVVCSelectPO from "../Modals/DVVCSelectPO";
import TextNumber, { FormatedNumber } from "../Common/TextNumber";
import { GetListCompany } from "../../Services/CompanyService";
import HeaderPage from "../Common/HeaderPage";
import { AddVehicleRegisterForShip, GetDSXeGiaoTheoTau, UpdateVehicleRegisterForShip } from "../../Services/KhaiBaoService/KhaiBaoTheoTau";
const { Search } = Input;
const { Option } = Select;
const layoutAddDriver = {
  labelCol: {
    span: 6,
  },
  wrapperCol: {
    span: 18,
  },
};
const dateFormat = "YYYY-MM-DD";

export class UpdateKhaiBaoTheoTau extends Component {
  static displayName = UpdateKhaiBaoTheoTau.name;

  // Check login
  componentWillMount() {
    myCheckAuth();
  }

  formRef = React.createRef();
  // [ form ] = Form.useForm()
  constructor(props) {
    super(props);
    this.state = {
      saving: false,
      detail: [],
      visibleOrderList: false,
      visibleOrderTargetList: false,
      visibleCreateDriver: false,
      visibleCreateVehicle: false,
      orderList: [],
      bsxList: [],
      romoocList: [],
      taiXeList: [],
      vat_tu: [],
      vat_tu_target: [],
      //
      allowSave: true,
      disableBtnAdd: true,
      fetchingBSX: false,
      gettingPO: true,
      isSelectedPlant: false,
      gettingPlant: true,
      listCompany: [],
      selectedPO: '',
    };
    this.setVisibleOrderList = this.setVisibleOrderList.bind(this);
    this.setVisibleOrderTargetList = this.setVisibleOrderTargetList.bind(this);
    this.showCreateDriver = this.showCreateDriver.bind(this);
    this.showCreateVehicle = this.showCreateVehicle.bind(this);
    this.handleCloseModal = this.handleCloseModal.bind(this);
  }

  setVisibleOrderList() {
    this.setState({
      visibleOrderList: !this.state.visibleOrderList,
    });
  }

  setVisibleOrderTargetList() {
    this.setState({
      visibleOrderTargetList: !this.state.visibleOrderTargetList,
    });
  }

  handleCloseModal() {
    this.setState({
      visibleOrderList: false,
      visibleOrderTargetList: false,
      visibleCreateDriver: false,
      visibleCreateVehicle: false,
    });
  }

  addDriver(record) {
    AddDriver(record).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        message.success(objRespone.err.msgString);
        this.handleCloseModal();
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  }

  // Bấm Ok ở modal chọn đơn hàng
  onSelectOrder = (record) => {
    console.log(record);
    if (record != undefined) {
      this.setVisibleOrderList();
      this.formRef.current.setFieldsValue({
        orderNumber: record.order,
        billNumber: record.billNumber,
        shipNumber: record.shipNumber,
        listVehicle: [],
      });

      this.setState({
        vat_tu: record.vattu,
        tlConLaiChoPhep: record.conLai,
        disableBtnAdd: false,
        selectedPO: record.order,
      });
    } else {
      alert("Vui lòng chọn đơn hàng");
    }
  };

  onSelectOrderTarget = (record) => {
    if (record != undefined) {
      this.setVisibleOrderTargetList();
      this.formRef.current.setFieldsValue({
        orderNumberTarget: record.order,
        billNumberTarget: record.billNumber,
        shipNumberTarget: record.shipNumber,
        // listVehicle: [],
      });
      this.setState({
        vat_tu_target: record.vattu,
        disableBtnAdd: false,
      });
    } else {
      alert("Vui lòng chọn đơn hàng");
    }
  };

  // Tìm kiếm biển số xe
  handleSearchBSX(text, type) {
    if (text.length >= 3) {
      this.setState({ fetchingBSX: true });
      SearchBSX(text, type).then((objRespone) => {
        if (objRespone.isSuccess === true) {
          if (type === "normal") {
            this.setState({
              bsxList: objRespone.data,
            });
          } else {
            this.setState({
              romoocList: objRespone.data,
            });
          }
        } else {
          this.setState({
            bsxList: [],
          });
        }
        this.setState({ fetchingBSX: false });
      });
    } else {
      this.setState({
        bsxList: [],
      });
    }
  }

  // Tìm kiếm Tên tài xế
  handleSearchTaiXe(text) {
    if (text.length >= 3) {
      SearchDriver(text).then((objRespone) => {
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

  componentDidMount() {
    // this.getCungDuong();
    // Lấy danh sách các plant
    this.getPlants();
  }

  showCreateDriver() {
    this.setState({
      visibleCreateDriver: true,
    });
  }

  showCreateVehicle() {
    this.setState({
      visibleCreateVehicle: true,
    });
  }

  // Lưu thông tin đăng ký
  onFinish = (values) => {
    // Đổi trạng thái của nút Lưu
    this.setState({
      saving: true,
    });
    // Kiểm tra đã nhập thông tin
    if (values.listVehicle === undefined) {
      alert("Chưa nhập thông tin vận chuyển");
      this.setState({ saving: false });
      return false;
    } else if (values.listVehicle.length === 0) {
      alert("Chưa nhập thông tin vận chuyển");
      this.setState({ saving: false });
      return false;
    }
    // Check Null BSX
    for (var k = 0; k < values.listVehicle.length; k++) {
      if (
        values.listVehicle[k].vehicleNumber === undefined ||
        values.listVehicle[k].vehicleNumber === null
      ) {
        alert("Biển số xe không đúng! Kiểm tra lại dòng thứ " + k + 1);
        this.setState({ saving: false });
        return false;
      }

      // kiểm tra nếu là xe đầu kéo thì phải có roomoc kéo theo
      if (values.listVehicle[k].isDauKeo) {
        if (
          values.listVehicle[k].romooc === undefined ||
          values.listVehicle[k].romooc === null
        ) {
          alert("Xe đầu kéo phải kèm theo rơ mooc, kiểm tra lại dòng " + k + 1);
          this.setState({ saving: false });
          return false;
        }
      }
    }

    // Update data đúng chuẩn
    values.startDate = values.startDate.format(dateFormat);

    console.log("Received values of form:", values);

    var valueToUpdate = {
      orderNumber: values.orderNumberTarget,
      shipNumber : values.shipNumberTarget,
      productCode: values.productCodeTarget,
      startDate: values.startDate,
      listVehicle: values.listVehicle,
    }

    // Call API Save data

    UpdateVehicleRegisterForShip(valueToUpdate).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        // message.success(objRespone.err.msgString);
        alert("Cập nhật thông tin thành công! Tải lại trang");
        window.location.reload();
      } else {
        message.error(objRespone.err.msgString);
      }
      this.setState({
        saving: false,
      });
    });
  };

  onSelectBSX = (value, key, option) => {
    var that = this;
    if (option.driver.driverId !== null) {
      var taiXe = {
        driverId: option.driver.driverId,
        driverName: option.driver.driverName,
        driverCardNo: option.driver.driverCardNo,
      };
      console.log(taiXe);

      this.setState(
        {
          taiXeList: [taiXe],
        },
        () => {
          // callback
          console.log("success setState");
          that.formRef.current.setFields([
            {
              name: [
                "listVehicle", // tên của list
                key, // vị trí
                "driverID1", // tên field
              ],
              value: option.driver.driverId, // giá trị gán
            },
          ]);
        }
      );
    }
    // Auto field Romooc
    if (option.romooc.vehicleNumber !== null) {
      var romooc = {
        vehicleId: option.romooc.vehicleId,
        vehicleNumber: option.romooc.vehicleNumber,
      };

      this.setState(
        {
          romoocList: [romooc],
        },
        () => {
          // callback
          that.formRef.current.setFields([
            {
              name: [
                "listVehicle", // tên của list
                key, // vị trí
                "romooc", // tên field
              ],
              value: option.romooc.vehicleNumber, // giá trị gán
            },
          ]);
        }
      );
    }

    // set giá trị isDauKeo
    that.formRef.current.setFields([
      {
        name: [
          "listVehicle", // tên của list
          key, // vị trí
          "isDauKeo", // tên field
        ],
        value: option.isDauKeo, // giá trị gán
      },
    ]);

  };

  changeCompanyHandler(value) {
    //
    console.log(value);
    // xóa thong tin đã nhập
    this.formRef.current.setFieldsValue({
      listVehicle: [],
      billNumber: "",
      shipNumber: "",
      orderNumber: "",
      billNumberTarget: "",
      shipNumberTarget: "",
      orderNumberTarget: "",
    });

    // Xóa danh sách PO
    this.setState({
      orderList: [],
    });

    this.setState({
      isSelectedPlant: true,
    });
    // Goi api lay Po
    this.getOrderDetail(value);
    // Show modal chọn Po
    this.setState({
      visibleOrderList: true,
    });
  }

  checkAllowedSave(changedValue, values) {
    console.log(changedValue.listVehicle);
    if (!changedValue.listVehicle) return 0;
    var len = changedValue.listVehicle.length || 0;
    if (changedValue.listVehicle[len - 1] == undefined) return 0;
    // console.log(changedValue.listVehicle[len - 1].trongLuongDuKienText)

  }

  render() {
    const optionsCompany = this.state.listCompany.map((item) => (
      <Option key={item.companyCode} value={item.companyCode}>
        {item.companyName}
      </Option>
    ));

    const optionsVatTu = this.state.vat_tu.map((item) => (
      <Option
        key={item.productCode}
      >
        {item.productName}
      </Option>
    ));

    const optionsVatTuTarget = this.state.vat_tu_target.map((item) => (
      <Option
        key={item.productCode}
      >
        {item.productName}
      </Option>
    ));

    const optionsBSX = this.state.bsxList.map((d) => (
      <Option
        key={d.vehicleId}
        value={d.vehicleNumber}
        isDauKeo={d.isDauKeo}
        driver={{
          driverId: d.driverId,
          driverName: d.driverName,
          driverCardNo: d.driverCardNo,
        }}
        romooc={{
          vehicleId: d.romoocId,
          vehicleNumber: d.romoocNumber,
        }}
      >
        {d.vehicleNumber}
      </Option>
    ));

    const optionsRomooc = this.state.romoocList.map((d) => (
      <Option key={d.vehicleId} value={d.vehicleNumber}>
        {d.vehicleNumber}
      </Option>
    ));

    const optionsTaiXe = this.state.taiXeList.map((d) => (
      <Option key={d.driverId}>
        {d.driverName} - {d.driverCardNo}
      </Option>
    ));

    return (
      <div>
        <HeaderPage
          title="Cập nhật danh sách xe chuyển SPL theo tàu"
          description="Cập nhật nhanh vật tư vận chuyển cho danh sách xe sẵng có"
        />
        <Form
          ref={this.formRef}
          onValuesChange={(changedValue, values) =>
            this.checkAllowedSave(changedValue, values)
          }
          // form={form}
          name="register_vehicel_giao_nhan"
          onFinish={this.onFinish}
          autoComplete="off"
        >
          {/* <Space align="baseline" style={{ width: '100%' }}> */}
          <Card
            style={{
              width: "100%",
              paddingLeft: "0px",
              border: "0.5px solid #d9d9d9",
              borderRadius: "10px",
            }}
          >
            <Row gutter={24}>
              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "5px", marginTop: "10px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  label="Nơi giao/nhận hàng"
                  name="companyCode"
                  rules={[
                    { required: true, message: "Chưa chọn nơi giao nhận" },
                  ]}
                >
                  {/* <Input disabled /> */}
                  <Select
                    // style={props.style}
                    defaultActiveFirstOption={true}
                    showArrow
                    filterOption={false}
                    onChange={(value) => this.changeCompanyHandler(value)}
                  >
                    {optionsCompany}
                  </Select>
                </Form.Item>
              </Col>
            </Row>
            <Row gutter={24}>
              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "5px", marginTop: "10px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  label="Mã đơn hàng"
                  name="orderNumber"
                  rules={[{ required: true, message: "Chưa chọn mã đơn hàng" }]}
                >
                  {/* <Input disabled /> */}
                  <Search
                    readOnly={true}
                    disabled={!this.state.isSelectedPlant}
                    onSearch={this.setVisibleOrderList}
                    style={{ width: "100%" }}
                  />
                </Form.Item>
              </Col>

              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "5px", marginTop: "10px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  label="Mã đơn hàng"
                  name="orderNumberTarget"
                  rules={[{ required: true, message: "Chưa chọn mã đơn hàng" }]}
                >
                  {/* <Input disabled /> */}
                  <Search
                    readOnly={true}
                    disabled={!this.state.isSelectedPlant}
                    onSearch={this.setVisibleOrderTargetList}
                    style={{ width: "100%" }}
                  />
                </Form.Item>
              </Col>
            </Row>

            <Row gutter={24}>
              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "5px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  label="Số bill"
                  name="billNumber"
                >
                  <Input disabled />
                </Form.Item>
              </Col>

              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "5px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  label="Số bill"
                  name="billNumberTarget"
                >
                  <Input disabled />
                </Form.Item>
              </Col>
            </Row>
            <Row gutter={24}>
              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "5px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  label="Tên tàu"
                  name="shipNumber"
                >
                  <Input disabled />
                </Form.Item>
              </Col>

              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "5px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  label="Tên tàu"
                  name="shipNumberTarget"
                >
                  <Input disabled />
                </Form.Item>
              </Col>
            </Row>
            <Row gutter={24}>
              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "5px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  label="Mặt hàng đang được khai báo"
                  name="productCodeSource"
                  rules={[{ required: true, message: "Chưa mặt hàng" }]}
                >
                  <Select
                    allowClear
                    notFoundContent={<>Chưa chọn đơn hàng</>}
                    onSelect={(value) => { this.getVehicleRegisterList(this.state.selectedPO, value) }}
                  >
                    {optionsVatTu}
                  </Select>
                </Form.Item>
              </Col>

              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "5px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  label="Mặt hàng sẽ giao"
                  name="productCodeTarget"
                  rules={[{ required: true, message: "Chưa mặt hàng" }]}
                >
                  <Select
                    allowClear
                    notFoundContent={<>Chưa chọn đơn hàng</>}
                  >
                    {optionsVatTuTarget}
                  </Select>
                </Form.Item>
              </Col>
            </Row>


            <Row gutter={24}>
              <Col span={12}>
                <Form.Item
                  style={{ marginBottom: "0px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  name="startDate"
                  label="Ngày giao/nhận"
                  initialValue={moment()}
                  rules={[
                    { required: true, message: "Chưa chọn ngày giao/nhận" },
                  ]}
                >
                  <DatePicker
                    style={{ width: "100%" }}
                    locale={locale}
                    placeholder=""
                    format={"DD/MM/YYYY"}
                  />
                </Form.Item>
              </Col>
            </Row>
          </Card>

          {/* </Space> */}
          <br />
          <br />

          {/* table-striped */}
          <table className="table" aria-labelledby="tabelLabel">
            <thead className="ant-table-thead">
              <tr>
                <th width="13%" style={{ minWidth: "120px" }}>
                  <a style={{ color: "red" }}>*</a> Biển số xe
                </th>
                <th width="0%" hidden></th>
                <th width="13%" style={{ minWidth: "120px" }}>
                  {" "}
                  Rơ mooc
                </th>
                <th width="30%" style={{ minWidth: "200px" }}>
                  <a style={{ color: "red" }}>*</a> Thông tin tài xế ca sáng
                </th>
                <th width="30%" style={{ minWidth: "200px" }}>
                  <a style={{ color: "red" }}>*</a> Thông tin tài xế ca tối
                </th>
                <th width="3%"></th>
              </tr>
            </thead>

            <Form.List name="listVehicle">
              {(fields, { add, remove }) => (
                <>
                  <tbody>
                    {fields.map((field) => (
                      <tr key={field.key}>
                        <td colSpan={8} style={{ padding: "0px" }}>
                          <table width="100%" className="table-borderless">
                            <tbody>
                              <tr style={{ backgroundColor: "#00000000" }}>
                                <td width="13%" style={{ minWidth: "120px" }}>
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
                                        name={[field.name, "vehicleNumber"]}
                                        fieldKey={[
                                          field.fieldKey,
                                          "vehicleNumber",
                                        ]}
                                        rules={[
                                          {
                                            required: true,
                                            message: "Chưa nhập BSX",
                                          },
                                        ]}
                                      >
                                        <Select
                                          showSearch
                                          // value={this.state.value}
                                          // placeholder="Nhập 3 ký tự"
                                          style={this.props.style}
                                          defaultActiveFirstOption={false}
                                          showArrow={false}
                                          filterOption={false}
                                          onSearch={(value) => {
                                            this.handleSearchBSX(
                                              value,
                                              "normal"
                                            );
                                          }}
                                          // onChange={this.handleChange}
                                          onChange={(value, option) =>
                                            this.onSelectBSX(
                                              value,
                                              field.key,
                                              option
                                            )
                                          }
                                          notFoundContent={
                                            this.state.fetchingBSX ? (
                                              <Spin size="small" />
                                            ) : (
                                              <Button
                                                onClick={this.showCreateVehicle}
                                              >
                                                Tạo mới
                                              </Button>
                                            )
                                          }
                                        >
                                          {optionsBSX}
                                        </Select>
                                      </Form.Item>
                                    )}
                                  </Form.Item>
                                </td>
                                <td hidden>
                                  <Form.Item
                                    hidden
                                    valuePropName="checked"
                                    {...field}
                                    name={[field.name, "isDauKeo"]}
                                    fieldKey={[field.fieldKey, "isDauKeo"]}
                                  // rules={[{ required: true, message: 'Chưa nhập BSX' }]}
                                  >
                                    <Checkbox />
                                  </Form.Item>
                                </td>
                                <td width="13%" style={{ minWidth: "120px" }}>
                                  <Form.Item
                                    style={{ marginBottom: "0px" }}
                                    {...field}
                                    name={[field.name, "romooc"]}
                                    fieldKey={[field.fieldKey, "romooc"]}
                                  >
                                    <Select
                                      showSearch
                                      style={this.props.style}
                                      defaultActiveFirstOption={false}
                                      showArrow={false}
                                      allowClear
                                      filterOption={false}
                                      onSearch={(value) => {
                                        this.handleSearchBSX(value, "romooc");
                                      }}
                                      // onChange={this.handleChange}
                                      // onChange={(value, option) => this.onSelectBSX(value, field.key, option)}
                                      notFoundContent={
                                        this.state.fetchingBSX ? (
                                          <Spin size="small" />
                                        ) : (
                                          <Button
                                            onClick={this.showCreateVehicle}
                                          >
                                            Tạo mới
                                          </Button>
                                        )
                                      }
                                    >
                                      {optionsRomooc}
                                    </Select>
                                  </Form.Item>
                                </td>
                                <td width="30%" style={{ minWidth: "200px" }}>
                                  <Form.Item
                                    style={{ marginBottom: "0px" }}
                                    {...field}
                                    name={[field.name, "driverID1"]}
                                    fieldKey={[field.fieldKey, "driverID1"]}
                                    rules={[
                                      {
                                        required: true,
                                        message: "Chưa nhập thông tin tài xế",
                                      },
                                    ]}
                                  >
                                    <Select
                                      showSearch
                                      // value={this.state.value}
                                      // placeholder="Nhập 3 ký tự"
                                      style={this.props.style}
                                      defaultActiveFirstOption={true}
                                      showArrow={false}
                                      filterOption={false}
                                      onSearch={(value) => {
                                        this.handleSearchTaiXe(value);
                                      }}
                                      // onChange={this.handleChange}
                                      notFoundContent={
                                        <Button onClick={this.showCreateDriver}>
                                          Tạo mới
                                        </Button>
                                      }
                                    >
                                      {optionsTaiXe}
                                    </Select>
                                  </Form.Item>
                                </td>
                                <td width="30%" style={{ minWidth: "200px" }}>
                                  <Form.Item
                                    style={{ marginBottom: "0px" }}
                                    {...field}
                                    name={[field.name, "driverID2"]}
                                    fieldKey={[field.fieldKey, "driverID2"]}
                                    rules={[
                                      {
                                        required: true,
                                        message: "Chưa nhập thông tin tài xế ca 2",
                                      },
                                    ]}
                                  >
                                    <Select
                                      showSearch
                                      style={this.props.style}
                                      defaultActiveFirstOption={true}
                                      showArrow={false}
                                      filterOption={false}
                                      onSearch={(value) => {
                                        this.handleSearchTaiXe(value);
                                      }}
                                      // onChange={this.handleChange}
                                      notFoundContent={
                                        <Button onClick={this.showCreateDriver}>
                                          Tạo mới
                                        </Button>
                                      }
                                    >
                                      {optionsTaiXe}
                                    </Select>
                                  </Form.Item>
                                </td>


                                <td
                                  width="3%"
                                  style={{ verticalAlign: "middle" }}
                                >
                                  <div className="my-popconfirm">
                                    <Popconfirm
                                      title="Bạn có muốn xoá？"
                                      icon={<WarningOutlined />}
                                      okText="Có"
                                      okType="default"
                                      cancelText="Không"
                                      onConfirm={() => remove(field.name)}
                                    >
                                      {/* <DeleteFilled /> */}
                                      <CloseOutlined style={{ color: "red" }} />
                                      {/* <img src={"/img/delete.svg"} /> */}
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
                  <Form.Item style={{ marginTop: "24px" }}>
                    <Button
                      disabled={this.state.disableBtnAdd}
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
            <Button
              type="primary"
              htmlType="submit"
              loading={this.state.saving}
            >
              Lưu thông tin
            </Button>
          </Form.Item>
        </Form>

        <i>
          Chú thích: <>&#91;</>
          <a style={{ color: "red" }}>*</a>
          <>&#93;</> là thông tin bắt buộc.
        </i>

        {/* Modal hiển thị chọn Order */}
        <DVVCSelectPO
          visible={this.state.visibleOrderList}
          onCancel={this.handleCloseModal}
          dataSource={this.state.orderList}
          onOk={this.onSelectOrder}
          loading={this.state.gettingPO}
        />

        {/* Modal hiển thị chọn Order muốn chuyển */}
        <DVVCSelectPO
          visible={this.state.visibleOrderTargetList}
          onCancel={this.handleCloseModal}
          dataSource={this.state.orderList}
          onOk={this.onSelectOrderTarget}
          loading={this.state.gettingPO}
        />

        {/* Tạo mới tài xế */}
        <Modal
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
          // Xoá data khi đóng
          destroyOnClose={true}
        >
          <Form
            {...layoutAddDriver}
            id="formAdd"
            onFinish={(values) => {
              this.addDriver(values);
            }}
          >
            <Form.Item
              labelAlign="left"
              name="driverName"
              label="Tên tài xế"
              normalize={(input) => input.toUpperCase()}
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
              labelAlign="left"
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
            {/* <Form.Item
              name="dob"
              label="Năm sinh"
              labelAlign="left"
            >
              <DatePicker picker="year" placeholder="Chọn năm sinh" style={{ width: '50%' }} />
            </Form.Item> */}
          </Form>
        </Modal>

        {/* Tạo mới Xe */}

        <CreateVehicle
          createModalVisible={this.state.visibleCreateVehicle}
          onCancel={this.handleCloseModal}
          onSuccess={this.handleCloseModal}
        />
      </div>
    );
  }


  // lấy danh sách xe cần chuyển vật tư

  getPOActive(objRespone) {
    // var isService = localStorage.getItem('isService') || false;
    var orders = [];
    // console.log('check' + isService)
    for (var i = 0; i < objRespone.item.poResponses.length; i++) {
      orders.push({
        key: objRespone.item.poResponses[i].pomasters.ponumber,
        provider: objRespone.item.poResponses[i].pomasters.providerName,
        order: objRespone.item.poResponses[i].pomasters.ponumber,
        total: objRespone.item.poResponses[i].pomasters.qtyTotal,
        vattu: objRespone.item.poResponses[i].polines,
        deliveryDate: objRespone.item.poResponses[i].polines[0].deliveryDate,
        documentDate: objRespone.item.poResponses[i].polines[0].documentDate,
        soLuongDaChuyen: objRespone.item.poResponses[i].trongLuongDaNhap,
        conLai: objRespone.item.poResponses[i]
          ? 0
          : objRespone.item.poResponses[i].pomasters.qtyTotal -
          objRespone.item.poResponses[i].registered -
          objRespone.item.poResponses[i].trongLuongDaNhap,
        registered:
          objRespone.item.poResponses[i].registered +
          objRespone.item.poResponses[i].trongLuongDaNhap,
        isGiaKhac: objRespone.item.poResponses[i].isGiaKhac,
        note: objRespone.item.poResponses[i].pomasters.note,
        billNumber: objRespone.item.poResponses[i].billNumber,
        soLuongCont: objRespone.item.poResponses[i].soLuongCont,
        shipNumber: objRespone.item.poResponses[i].shipNumber
      });
    }
    return orders;
  }

  setDSXe(lst) {
    var lstXe = lst.map((item) => ({
      id: item.id,
      vehicleNumber: item.vehicleNumber,
      romooc: item.romooc,
      driverID1: item.driverId1,
      driverID2: item.driverId2
    }));

    var driverSource = []
    lst.forEach(element => {
      driverSource.push({
        driverId: element.driverId1,
        driverName: element.driverName1,
        driverCardNo: element.driverIdCard1
      })
      if (element.driverID2 !== null) {
        driverSource.push({
          driverId: element.driverId2,
          driverName: element.driverName2,
          driverCardNo: element.driverIdCard2
        })
      }

    });

    this.setState({
      taiXeList: driverSource
    })


    this.formRef.current.setFieldsValue({
      listVehicle: [],
    });


    this.formRef.current.setFieldsValue({
      listVehicle: lstXe,
    })
  }

  async getVehicleRegisterList(poNumber, productCode) {
    await GetDSXeGiaoTheoTau(poNumber, '', productCode).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        console.log(objRespone.data);
        this.setDSXe(objRespone.data)
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  }

  async getOrderDetail(plant) {
    this.setState({
      gettingPO: true,
    });
    await GetActiveOrderList(plant).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        var order = this.getPOActive(objRespone);
        var isService = localStorage.getItem("isService") || false;
        var order2Wiew = [];
        var countGoiDau = 0;

        // Nếu là dịch vụ vận chuyển thì hiện toàn bộ
        if (isService == "true") {
          order2Wiew = order;
        } else {
          // Không hiện PO gối đầu đối với nhà cung cấp
          for (var j = 0; j < order.length; j++) {
            // Nếu PO có deliveryDate là hôm nay thì hiện hết lên
            if (
              moment(order[j].deliveryDate).format("DD/MM/YYYY") ==
              moment().format("DD/MM/YYYY")
            ) {
              order2Wiew = order;
              break;
            } else {
              // Nếu bình thường thì chỉ hiện 1 PO và PO giá khác
              if (order[j].isGiaKhac) {
                order2Wiew.push(order[j]);
              } else if (countGoiDau < 1) {
                order2Wiew.push(order[j]);
                countGoiDau++;
              }
            }
          }
        }

        // console.log(isService)
        this.setState({
          orderList: order2Wiew,
        });
      } else {
        // message.error(objRespone.err.msgString)
      }
      this.setState({
        gettingPO: false,
      });
    });
  }

  async getPlants() {
    this.setState({
      gettingPlant: true,
    });
    await GetListCompany().then((objRespone) => {
      if (objRespone.isSuccess === true) {
        this.setState({
          listCompany: objRespone.data,
        });
      } else {
        message.error(objRespone.err.msgString);
      }
    });
    this.setState({
      gettingPlant: false,
    });
  }

}
