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
import { myCheckAuth } from "../../../Services/authServices";
import { GetActiveOrderList } from "../../../Services/OrderServices/orderService";
// import { GetCungDuongList } from '../../Services/CungDuongService/cungDuongService';
import {
  AddVehicle,
  SearchBSX,
} from "../../../Services/VehicleService/vehicleService";
import {
  AddDriver,
  SearchDriver,
} from "../../../Services/DriverService/driverService";
import { AddVehicleRegisterPO } from "../../../Services/KhaiBaoService/khaiBaoService";
import moment from "moment";
import "moment/locale/vi";
import "./index.less";
import locale from "antd/es/date-picker/locale/vi_VN";
import CreateVehicle from "../../Modals/CreateVehicle";
import DVVCSelectPO from "../../Modals/DVVCSelectPO";
import TextNumber, { FormatedNumber } from "../../Common/TextNumber";
import { GetListCompany } from "../../../Services/CompanyService";
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

export class KhaiBaoThongTinDVVC extends Component {
  static displayName = KhaiBaoThongTinDVVC.name;

  // Check login
  componentWillMount() {
    myCheckAuth();
  }

  formRef = React.createRef();
  // [ form ] = Form.useForm()
  constructor(props) {
    super(props);
    this.state = {
      forecasts: [],
      saving: false,
      detail: [],
      visibleOrderList: false,
      visibleCreateDriver: false,
      visibleCreateVehicle: false,
      orderList: [],
      cungDuongList: [],
      bsxList: [],
      romoocList: [],
      taiXeList: [],
      vat_tu: [],
      //
      allowSave: true,
      tlConLaiChoPhep: 0,
      disableBtnAdd: true,
      fetchingBSX: false,
      gettingPO: true,
      isSelectedPlant: false,
      gettingPlant: true,
      listCompany: [],
    };
    this.setVisibleOrderList = this.setVisibleOrderList.bind(this);
    this.showCreateDriver = this.showCreateDriver.bind(this);
    this.showCreateVehicle = this.showCreateVehicle.bind(this);
    this.handleCloseModal = this.handleCloseModal.bind(this);
  }

  setVisibleOrderList() {
    this.setState({
      visibleOrderList: !this.state.visibleOrderList,
    });
  }

  handleCloseModal() {
    this.setState({
      visibleOrderList: false,
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
        listVehicle: [],
      });
      this.setState({
        vat_tu: record.vattu,
        tlConLaiChoPhep: record.conLai,
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
      console.log(values.listVehicle[k].isDauKeo);
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
    values.ngayGiaoNhan = values.ngayGiaoNhan.format(dateFormat);
    values.cungDuongCode = parseInt(values.cungDuongCode);
    for (var i = 0; i < values.listVehicle.length; i++) {
      if (
        values.listVehicle[i].trongLuongDuKienText === undefined ||
        values.listVehicle[i].trongLuongDuKienText === null
      ) {
        values.listVehicle[i].trongLuongDuKien = 0;
      } else
        values.listVehicle[i].trongLuongDuKien = parseInt(
          values.listVehicle[i].trongLuongDuKienText
        );
    }
    console.log("Received values of form:", values);

    // Call API Save data
    AddVehicleRegisterPO(values).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        // message.success(objRespone.err.msgString);
        alert("Lưu thông tin thành công! Tải lại trang");
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
                "driverID", // tên field
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

    // this.setState({
    //   taiXeList: [taiXe]
    // }), () => { // callback
    //   console.log('success setState');
    //   this.formRef.current.setFields([
    //     {
    //       name: [
    //         'listVehicle', // tên của list
    //         key,// vị trí
    //         'driverID' // tên field
    //       ],
    //       value: value // giá trị gán
    //     }
    //   ])
    // }
  };

  changeCompanyHandler(value) {
    //
    console.log(value);
    // xóa thong tin đã nhập
    this.formRef.current.setFieldsValue({
      listVehicle: [],
      billNumber: "",
      orderNumber: "",
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
    if (changedValue.listVehicle[len - 1].trongLuongDuKienText == undefined)
      return 0;

    // console.log(changedValue.listVehicle[len - 1].trongLuongDuKienText)

    // lấy trọng lượng cho phép
    var tlDangDK = 0;
    for (var i = 0; i < values.listVehicle.length; i++) {
      if (values.listVehicle[i])
        tlDangDK += values.listVehicle[i].trongLuongDuKienText;
    }
    if (this.state.tlConLaiChoPhep < tlDangDK) {
      // message.error("Vượt trọng lượng giao cho phép")
      var khaDungNhap =
        this.state.tlConLaiChoPhep -
        tlDangDK +
        changedValue.listVehicle[len - 1].trongLuongDuKienText;
      message.destroy();
      message.warning(
        "Trọng lượng còn lại cho phép: " +
          new Intl.NumberFormat("de-DE").format(khaDungNhap) +
          "kg"
      );

      // this.formRef.current.setFields([
      //   {
      //     name: [
      //       'listVehicle', // tên của list
      //       len - 1,// vị trí
      //       'trongLuongDuKienText' // tên field
      //     ],
      //     // validating: true
      //     help: '1111'

      //   }
      // ])
    } else {
      this.formRef.current.setFields([
        {
          name: [
            "listVehicle", // tên của list
            len - 1, // vị trí
            "trongLuongDuKienText", // tên field
          ],
          errors: [],
        },
      ]);
    }
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
        // value={
        //   {
        //     productCode:  item.productCode,
        //     productName:  item.productName
        //   }
        // }
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

    const optionsCungDuong = this.state.cungDuongList.map((cungDuong) => (
      <Option key={cungDuong.cungDuongCode}>{cungDuong.cungDuongName}</Option>
    ));

    return (
      <div>
        <h2 id="tabelLabel">{localStorage.getItem("company")}</h2>
        <p>Khai báo thông tin xe giao/nhận cho đơn hàng</p>
        <br />
        <br />
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
              <Col span={13}>
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
              <Col span={13}>
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
            </Row>

            <Row gutter={24}>
              <Col span={13}>
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
            </Row>
            <Row gutter={24}>
              <Col span={13}>
                <Form.Item
                  style={{ marginBottom: "0px" }}
                  labelAlign="left"
                  labelCol={{ span: 8 }}
                  name="ngayGiaoNhan"
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
              {/* <Col span={14}>
              <Form.Item
                // labelCol={{ span: 4 }}
                label="Chọn cung đường"
                name='cungDuongCode'
              >
                <Select
                  // style={{ width: '100%' }}
                  showSearch
                  allowClear
                  placeholder="Nhập để tìm kiếm"
                  optionFilterProp="children"
                  filterOption={(input, option) =>
                    option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                  }
                  filterSort={(optionA, optionB) =>
                    optionA.children.toLowerCase().localeCompare(optionB.children.toLowerCase())
                  }
                >
                  {optionsCungDuong}
                </Select>
              </Form.Item>

            </Col> */}
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
                  <a style={{ color: "red" }}>*</a> Thông tin tài xế
                </th>
                <th width="20%" style={{ minWidth: "200px" }}>
                  <a style={{ color: "red" }}>*</a> Mặt hàng
                </th>
                <th width="13%" style={{ minWidth: "120px" }}>
                  Trọng lượng hàng, kg
                </th>
                <th width="6%" style={{ minWidth: "30px" }}>
                  <Tooltip title="Số lượt xe ra vào nhà máy">
                    <span>Số lượt</span>
                  </Tooltip>
                </th>
                <th width="3%"></th>
              </tr>
            </thead>

            <Form.List name="listVehicle">
              {(fields, { add, remove }) => (
                <>
                  <tbody>
                    {fields.map((field) => (
                      // <Space key={field.key}
                      //   align="baseline"
                      //   direction ="vertical"
                      // >
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
                                    name={[field.name, "driverID"]}
                                    fieldKey={[field.fieldKey, "driverID"]}
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
                                <td width="20%" style={{ minWidth: "200px" }}>
                                  <Form.Item
                                    style={{ marginBottom: "0px" }}
                                    {...field}
                                    name={[field.name, "listProduct"]}
                                    fieldKey={[field.fieldKey, "listProduct"]}
                                    rules={[
                                      {
                                        required: true,
                                        message: "Chưa chọn mặt hàng",
                                      },
                                    ]}
                                  >
                                    <Select
                                      mode="multiple"
                                      allowClear
                                      notFoundContent={<>Chưa chọn đơn hàng</>}
                                      // placeholder="Please select"
                                      // defaultValue={}
                                      // onChange={handleChange}
                                    >
                                      {optionsVatTu}
                                    </Select>
                                  </Form.Item>
                                </td>
                                <td width="13%" style={{ minWidth: "120px" }}>
                                  <Form.Item
                                    style={{ marginBottom: "0px" }}
                                    {...field}
                                    name={[field.name, "trongLuongDuKienText"]}
                                    fieldKey={[
                                      field.fieldKey,
                                      "trongLuongDuKienText",
                                    ]}
                                    // help="Xe quá tải"
                                    rules={[
                                      {
                                        type: "number",
                                        // min: 1,
                                        max: 99999,
                                        message: "Vui lòng nhập đúng giá trị",
                                      },
                                    ]}
                                  >
                                    {/* <Input min={1} max={99999} type='number' style={{ width: '100%', textAlign: 'right' }}  /> */}
                                    <InputNumber
                                      className="my-input-number"
                                      style={{ width: "100%" }}
                                      suffix="KG"
                                      formatter={(value) =>
                                        FormatedNumber(value)
                                      }
                                                        parser={TextNumber.weightParser}
                                                        disabled={true}
                                                        readOnly
                                    />
                                  </Form.Item>
                                </td>
                                <td width="6%" style={{ minWidth: "50px" }}>
                                  <Form.Item
                                    style={{ marginBottom: "0px" }}
                                    {...field}
                                    name={[field.name, "soLuot"]}
                                    fieldKey={[field.fieldKey, "soLuot"]}
                                    initialValue={1}
                                    // help="Xe quá tải"
                                    rules={[
                                      {
                                        type: "number",
                                        min: 1,
                                        max: 15,
                                        message: "Nhỏ hơn 15",
                                      },
                                    ]}
                                  >
                                    {/* <Input min={1} max={99999} type='number' style={{ width: '100%', textAlign: 'right' }}  /> */}
                                    <InputNumber
                                      className="my-input-number"
                                      style={{ width: "100%" }}
                                      formatter={(value) =>
                                        FormatedNumber(value)
                                      }
                                      parser={TextNumber.weightParser}
                                    />
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
    //Lê Hoàng Long
    //Fix lỗi không có trọng lượng còn lại của PO user dịch vụ vận chuyển
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
        conLai:
            objRespone.item.poResponses[i].pomasters.qtyTotal -
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
    // }
    // if (objRespone.item.poResponses[0]) {
    //   // console.log('uncheck' + isService)
    //   orders.push({
    //     key: objRespone.item.poResponses[0].pomasters.ponumber,
    //     provider: objRespone.item.poResponses[0].pomasters.providerName,
    //     order: objRespone.item.poResponses[0].pomasters.ponumber,
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

  // async getCungDuong() {
  //   console.log('Get Cung Duong');
  //   await GetCungDuongList().then((objRespone) => {
  //     if (objRespone.isSuccess === true) {
  //       this.setState({
  //         cungDuongList: objRespone.data
  //       })

  //     } else {
  //       // message.error(objRespone.err.msgString)
  //     }
  //   })
  // }
}
