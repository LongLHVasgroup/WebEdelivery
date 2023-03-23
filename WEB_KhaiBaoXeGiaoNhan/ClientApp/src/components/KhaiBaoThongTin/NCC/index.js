import React, { useContext, useEffect, useState, useRef } from "react";
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
import HeaderPage from "../../Common/HeaderPage";
import {
  CheckOutlined,
  PlusOutlined,
  WarningOutlined,
  CloseOutlined,
} from "@ant-design/icons";
import { GetActiveOrderList } from "../../../Services/OrderServices/orderService";
// import { GetCungDuongList } from '../../Services/CungDuongService/cungDuongService';
import {
  // AddVehicle,
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
import TextNumber, {
  FormatedNumber,
  WeightParser,
} from "../../Common/TextNumber";
import { GetUserInFo } from "../../../Services/AccountSevice/AccountService";
import AuthContext from "../../../store/auth-context";
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
var latestCall = 0;

const KhaiBaoThongTin = (props) => {
  // Lấy context
  const authCtx = useContext(AuthContext);

  const formRef = useRef();

  // const [companyName, setCompanyName] = useState("");
  const [listPO, setListPO] = useState([]);

  const [listBSX, setListBSX] = useState([]);
  const [listRomooc, setListRomooc] = useState([]);
  const [listTaiXe, setListTaiXe] = useState([]);
  const [listVatTu, setListVatTu] = useState([]);
  const [isShowModalSelectPO, setIsShowModalSelectPO] = useState(false);
  const [isShowModalCreateTaiXe, setIsShowModalCreateTaiXe] = useState(false);
  const [isShowModalCreateVehicle, setIsShowModalCreateVehicle] = useState(
    false
  );
  const [isSaving, setIsSaving] = useState(false);
  const [isDisableAddBtn, setIsDisableAddBtn] = useState(true);
  // const [selectedRowKeyPO, setSelectedRowKeyPO] = useState();
  const [isFetchingPO, setIsFetchingPO] = useState(false);
  const [tlConLaiChoPhep, setTLConLaiChoPhep] = useState(0);
  const [selectedPO, setSelectedPO] = useState();
  // const [poNumberOnChange, setPONumberOnChange] = useState("");
  const [mySelectionRowKey, setMySelectionRowKey] = useState();
  const [tempSelectPO, setTempSelectPO] = useState();
  const [isFetchingBSX, setIsFetchingBSX] = useState(false);
  const [isFetchingRomooc, setIsFetchingRomooc] = useState(false);
  const [isFetchingTaiXe, setIsFetchingTaiXe] = useState(false);
  const [textSearchBSX, setTextSearchBSX] = useState("");
  const [textSearchRomooc, setTextSearchRomooc] = useState("");
  const [textSearchTaiXe, setTextSearchTaiXe] = useState("");
  const [myCompany, setMyCompany] = useState("");
  const [selectedCompanyCode, setSelectedCompanyCode] = useState();
  const [listCompany, setListCompany] = useState([]);
  // const [latestCall, setLatestCall] = useState(0);

  // ================================ API Caller

  // Kiểm tra Authen còn hạn
  useEffect(() => {
    authCtx.checkAuth();
  }, []);

  // Lấy danh sách Cty
  useEffect(() => {
    GetListCompany().then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setListCompany(objRespone.data);
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  }, []);

  useEffect(() => {
    if (selectedCompanyCode === undefined) return;
    console.log("getPO");

    latestCall++;
    var callTimes = latestCall;
    // setLatestCall(callTimes);
    setIsFetchingPO(true);
    GetActiveOrderList(selectedCompanyCode, callTimes)
      .then((objRespone) => {
        console.log(callTimes, latestCall);
        if (callTimes === latestCall) {
          if (objRespone.isSuccess === true) {
            console.log("latest");
            var order = mappingPOActive(objRespone);
            var order2Wiew = [];
            order2Wiew = order;
            setListPO(order2Wiew);
            // }
          } else {
            message.error(objRespone.err.msgString);
          }
          setIsFetchingPO(false);
        }
      })
      .catch(() => console.error("Promise rejected!"));

    // Callback
    return () => {
      // setLatestCall(0);
    };
  }, [selectedCompanyCode]);

  // Lấy thông tin cá nhân
  useEffect(() => {
    GetUserInFo(authCtx.username).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setMyCompany(objRespone.item.company);
      } else {
        message.error(objRespone.err.msgString);
      }
    });
    // return () => {
    //   clearTimeout(searchTaiXeTimeout);
    // };
  }, []);

  // Tìm kiếm biển số xe
  useEffect(() => {
    console.log(textSearchBSX);
    var bsx = textSearchBSX.trim();
    if (bsx.length >= 3) {
      const searchTaiXeTimeout = setTimeout(() => {
        fetchBSX(bsx, "normal");
      }, 500);

      return () => {
        clearTimeout(searchTaiXeTimeout);
      };
    }
  }, [textSearchBSX]);

  // Tìm Romooc
  useEffect(() => {
    console.log(textSearchRomooc);
    var romooc = textSearchRomooc.trim();
    if (romooc.length >= 3) {
      const searchRomoocTimeout = setTimeout(() => {
        fetchBSX(romooc, "romooc");
      }, 500);

      return () => {
        clearTimeout(searchRomoocTimeout);
      };
    }
  }, [textSearchRomooc]);

  // Tìm kiếm Tài xế
  useEffect(() => {
    console.log(textSearchTaiXe);
    var taiXe = textSearchTaiXe.trim();
    if (taiXe.length >= 3) {
      const searchTaiXeTimeout = setTimeout(() => {
        fetchTaiXe(taiXe);
      }, 500);

      return () => {
        clearTimeout(searchTaiXeTimeout);
      };
    }
  }, [textSearchTaiXe]);

  // =================================

  // Fetch danh sach don hang theo plant
  // const getListPO = async () => {
  //   setIsFetchingPO(true);
  //   await GetActiveOrderList().then((objRespone) => {
  //     if (objRespone.isSuccess === true) {
  //       var order = mappingPOActive(objRespone);
  //       var order2Wiew = [];
  //       order2Wiew = order;
  //       setListPO(order2Wiew);
  //     } else {
  //       message.error(objRespone.err.msgString);
  //     }
  //     setIsFetchingPO(false);
  //   });
  // };

  // TK thoogn tin trước khi save
  const checkAllowedSave = (changedValue, values) => {
    if (!changedValue.listVehicle) return 0;
    var len = changedValue.listVehicle.length || 0;
    if (changedValue.listVehicle[len - 1] == undefined) return 0;
    if (changedValue.listVehicle[len - 1].trongLuongDuKienText == undefined)
      return 0;

    // lấy trọng lượng cho phép
    var tlDangDK = 0;
    for (var i = 0; i < values.listVehicle.length; i++) {
      if (values.listVehicle[i])
        tlDangDK += values.listVehicle[i].trongLuongDuKienText;
    }
    if (tlConLaiChoPhep < tlDangDK) {
      // message.error("Vượt trọng lượng giao cho phép")
      var khaDungNhap =
        tlConLaiChoPhep -
        tlDangDK +
        changedValue.listVehicle[len - 1].trongLuongDuKienText;
      message.destroy();
      message.warning(
        "Trọng lượng còn lại cho phép: " +
          new Intl.NumberFormat("de-DE").format(khaDungNhap) +
          "kg"
      );
    } else {
      formRef.current.setFields([
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
  };

  const onFinishRegisterHandler = (values) => {
    // Đổi trạng thái của nút Lưu
    setIsSaving(true);
    // Kiểm tra đã nhập thông tin
    console.log(values);
    if (values.listVehicle === undefined) {
      alert("Chưa nhập thông tin vận chuyển");
      setIsSaving(false);
      return false;
    } else if (values.listVehicle.length === 0) {
      alert("Chưa nhập thông tin vận chuyển");
      setIsSaving(false);
      return false;
    }
    // Check Null BSX
    for (var k = 0; k < values.listVehicle.length; k++) {
      if (
        values.listVehicle[k].vehicleNumber === undefined ||
        values.listVehicle[k].vehicleNumber === null
      ) {
        alert("Biển số xe không đúng! Kiểm tra lại dòng thứ " + k + 1);
        setIsSaving(false);
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
          setIsSaving(false);
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
      setIsSaving(false);
    });
  };

  // Bấm vào 1 dòng trên modal chọn PO
  const onClickOrderLine = (record) => {
    // console.log(record);
    setMySelectionRowKey([record.key]);
    // lưu vào biến tạm trước khi bấm nút chọn
    setTempSelectPO(record);
  };

  // Mappign lại data po nhận được
  const mappingPOActive = (objRespone) => {
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
        billNumber: objRespone.item.poResponses[i].pomasters.note.slice(
          1,
          objRespone.item.poResponses[i].pomasters.note.slice(1).indexOf("*")
        ),
      });
    }
    return orders;
  };

  // Tạo mới Tài xế
  const onFinishCreateDriverHandle = (record) => {
    AddDriver(record).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        message.success(objRespone.err.msgString);
        setIsShowModalCreateTaiXe(false);
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  };

  // Bấm Ok ở modal chọn đơn hàng
  const onFinishSelectPOHandler = () => {
    if (tempSelectPO) {
      // set value leen form
      formRef.current.setFieldsValue({
        orderNumber: tempSelectPO.order,
        listVehicle: [],
      });
      console.log(tempSelectPO);
      // Set danh sách vật tư lên option
      setListVatTu(tempSelectPO.vattu);
      setTLConLaiChoPhep(tempSelectPO.conLai);
      setSelectedPO(tempSelectPO.order);
      setIsDisableAddBtn(false);
      setIsShowModalSelectPO(false);
    } else {
      alert("Vui lòng chọn đơn hàng");
    }
  };

  const onChangeBSXHandler = (value, key, option) => {
    if (option.driver.driverId !== null) {
      var taiXe = {
        driverId: option.driver.driverId,
        driverName: option.driver.driverName,
        driverCardNo: option.driver.driverCardNo,
      };

      setListTaiXe([taiXe]);
      formRef.current.setFields([
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

    // set Romooc tự động
    if (option.romooc.vehicleId !== null) {
      var romooc = {
        vehicleId: option.romooc.vehicleId,
        vehicleNumber: option.romooc.vehicleNumber,
      };

      setListRomooc([romooc]);
      formRef.current.setFields([
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

    // set là xe đầu kéo
    formRef.current.setFields([
      {
        name: [
          "listVehicle", // tên của list
          key, // vị trí
          "isDauKeo", // tên field
        ],
        value: option.daukeo.isDauKeo, // giá trị gán
      },
    ]);
  };

  // Tìm kiếm biển số xe
  const fetchBSX = (text, type) => {
    setIsFetchingBSX(true);
    setIsFetchingRomooc(true);

    SearchBSX(text, type).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        console.log(objRespone.data);
        if (type === "normal") {
          setListBSX(objRespone.data);
        } else {
          setListRomooc(objRespone.data);
        }
      } else {
        setListBSX([]);
        setListRomooc([]);
      }
      setIsFetchingBSX(false);
      setIsFetchingRomooc(false);
    });
  };

  // Tìm kiếm biển số xe
  const fetchTaiXe = (text) => {
    setIsFetchingTaiXe(true);
    SearchDriver(text).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setListTaiXe(objRespone.data);
      } else {
        setListTaiXe([]);
      }
      setIsFetchingTaiXe(false);
    });
  };

  const columns = [
    {
      title: (
        <>
          Mã số
          <br />
          đơn hàng
        </>
      ),
      dataIndex: "order",
      render: (text, row) => (
        <Tooltip
          title={
            localStorage.getItem("isService") == "true"
              ? row.note
              : row.provider
          }
        >
          <span>{text}</span>
        </Tooltip>
      ),
      width: "20%",
    },
    {
      title: (
        <>
          Thời hạn
          <br />
          hiệu lực
        </>
      ),
      dataIndex: "deliveryDate",
      render: (text) => moment(text).format("DD/MM/YYYY"),
      align: "right",
      width: "20%",
    },
    {
      title: "Số lượng, kg",
      children: [
        {
          title: "Đơn hàng",
          dataIndex: "total",
          align: "right",
          render: (text) => <TextNumber value={text} />,
          // width: '20%'
        },
        {
          title: "Đã khai báo",
          dataIndex: "registered",
          align: "right",
          render: (text) =>
            text === null ? <>&#8210;</> : <TextNumber value={text} />,
          // width: '20%'
        },
        {
          title: "Còn lại",
          align: "right",
          dataIndex: "conLai",
          render: (text) => <TextNumber value={text} />,
          // width: '20%',
        },
      ],
    },
    {
      title: "Giá khác",
      align: "center",
      dataIndex: "isGiaKhac",
      render: (text) => text && <CheckOutlined />,
      //
      // width: '20%',
    },
  ];

  //
  const changeCompanyHandler = (value) => {
    setSelectedCompanyCode(value);
    setTempSelectPO(undefined);
    // xóa data input
    formRef.current.setFieldsValue({
      orderNumber: "",
      listVehicle: [],
    });
    setMySelectionRowKey([]);
    // console.log(tempSelectPO);
    // Set danh sách vật tư lên option
    setListVatTu([]);
    setTLConLaiChoPhep(0);
    setSelectedPO("");
    setIsDisableAddBtn(true);
    setIsShowModalSelectPO(true);
  };

  const optionsTaiXe = listTaiXe.map((d) => (
    <Option key={d.driverId}>
      {d.driverName} - {d.driverCardNo}
    </Option>
  ));
  const optionsVatTu = listVatTu.map((item) => (
    <Option key={item.productCode}>{item.productName}</Option>
  ));

  const optionsBSX = listBSX.map((d) => (
    <Option
      key={d.vehicleId}
      value={d.vehicleNumber}
      daukeo={{ isDauKeo: d.isDauKeo }}
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

  const optionsRomooc = listRomooc.map((d) => (
    <Option key={d.vehicleId} value={d.vehicleNumber}>
      {d.vehicleNumber}
    </Option>
  ));

  const optionsPlant = listCompany.map((company) => (
    <Option key={company.companyCode} value={company.companyCode}>
      {company.companyName}
    </Option>
  ));

  return (
    <React.Fragment>
      <HeaderPage
        title={myCompany}
        description="Khai báo thông tin xe giao/nhận cho đơn hàng"
      />

      <Form
        ref={formRef}
        onValuesChange={(changedValue, values) =>
          checkAllowedSave(changedValue, values)
        }
        name="register_vehicel_giao_nhan"
        onFinish={onFinishRegisterHandler}
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
            <Col span={13}>
              <Form.Item
                style={{ marginBottom: "5px", marginTop: "10px" }}
                labelAlign="left"
                labelCol={{ span: 8 }}
                label="Nơi giao/nhận hàng"
                name="companyCode"
                rules={[{ required: true, message: "Chưa chọn nơi giao nhận" }]}
              >
                {/* <Input disabled /> */}
                <Select
                  style={props.style}
                  defaultActiveFirstOption={true}
                  showArrow
                  filterOption={false}
                  onChange={changeCompanyHandler}
                >
                  {optionsPlant}
                </Select>
              </Form.Item>
            </Col>
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
                  disabled={selectedCompanyCode !== undefined ? false : true}
                  onSearch={() => setIsShowModalSelectPO(true)}
                  style={{ width: "100%" }}
                />
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
              <th width="25%" style={{ minWidth: "200px" }}>
                <a style={{ color: "red" }}>*</a> Mặt hàng
              </th>
              <th width="17%" style={{ minWidth: "120px" }}>
                Trọng lượng hàng, kg
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
                      <td colSpan={7} style={{ padding: "0px" }}>
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
                                        style={props.style}
                                        defaultActiveFirstOption={false}
                                        showArrow={false}
                                        filterOption={false}
                                        onSearch={(value) => {
                                          setTextSearchBSX(value);
                                        }}
                                        onChange={(value, option) =>
                                          onChangeBSXHandler(
                                            value,
                                            field.key,
                                            option
                                          )
                                        }
                                        notFoundContent={
                                          isFetchingBSX ? (
                                            <Spin size="small" />
                                          ) : (
                                            <Button
                                              onClick={() =>
                                                setIsShowModalCreateVehicle(
                                                  true
                                                )
                                              }
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
                                    style={props.style}
                                    defaultActiveFirstOption={false}
                                    showArrow={false}
                                    allowClear
                                    filterOption={false}
                                    onSearch={(value) => {
                                      setTextSearchRomooc(value);
                                    }}
                                    // onChange={this.handleChange}
                                    // onChange={(value, option) => this.onSelectBSX(value, field.key, option)}
                                    notFoundContent={
                                      isFetchingRomooc ? (
                                        <Spin size="small" />
                                      ) : (
                                        <Button
                                          onClick={() =>
                                            setIsShowModalCreateVehicle(true)
                                          }
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
                                    style={props.style}
                                    defaultActiveFirstOption={true}
                                    showArrow={false}
                                    filterOption={false}
                                    onSearch={(value) =>
                                      setTextSearchTaiXe(value)
                                    }
                                    notFoundContent={
                                      <Button
                                        onClick={() =>
                                          setIsShowModalCreateTaiXe(true)
                                        }
                                      >
                                        Tạo mới
                                      </Button>
                                    }
                                  >
                                    {optionsTaiXe}
                                  </Select>
                                </Form.Item>
                              </td>
                              <td width="25%" style={{ minWidth: "200px" }}>
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
                              <td width="17%" style={{ minWidth: "120px" }}>
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
                                  <InputNumber
                                    className="my-input-number"
                                    style={{ width: "100%" }}
                                    suffix="KG"
                                    formatter={(value) => FormatedNumber(value)}
                                    parser={WeightParser}
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
                                    <CloseOutlined style={{ color: "red" }} />
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
                    disabled={isDisableAddBtn}
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
          <Button type="primary" htmlType="submit" loading={isSaving}>
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
      <Modal
        title="Chọn mã đơn hàng"
        centered
        visible={isShowModalSelectPO}
        onOk={() => onFinishSelectPOHandler()}
        okText="Chọn"
        cancelText="Huỷ"
        onCancel={() => setIsShowModalSelectPO(false)}
        width={800}
      >
        <Table
          className="my-table-order-list"
          locale={{ emptyText: "Không có dữ liệu" }}
          rowSelection={{
            selectedRowKeys: mySelectionRowKey,
            type: "radio",
            onChange: (selectedRowKeys, selectedRows) => {
              onClickOrderLine(selectedRows[0]);
            },
          }}
          onRow={(record, rowIndex) => {
            return {
              onClick: (event) => {
                onClickOrderLine(record);
              }, // click row
            };
          }}
          columns={columns}
          dataSource={listPO}
          bordered
          pagination={false}
          loading={isFetchingPO}
        />
      </Modal>

      {/* Tạo mới tài xế */}
      <Modal
        visible={isShowModalCreateTaiXe}
        title="Thêm tài xế"
        onCancel={() => setIsShowModalCreateTaiXe(false)}
        footer={[
          <Button type="primary" form="formAdd" key="submit" htmlType="submit">
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
            onFinishCreateDriverHandle(values);
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
        </Form>
      </Modal>

      {/* Tạo mới Xe */}
      <CreateVehicle
        createModalVisible={isShowModalCreateVehicle}
        onCancel={() => setIsShowModalCreateVehicle(false)}
        onSuccess={() => setIsShowModalCreateVehicle(false)}
      />
    </React.Fragment>
  );
};

export default KhaiBaoThongTin;
