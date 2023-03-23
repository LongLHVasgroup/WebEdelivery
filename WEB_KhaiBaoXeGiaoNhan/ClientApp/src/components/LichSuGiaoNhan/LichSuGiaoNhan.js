import {
  Button, DatePicker,
  Divider,
  Form, Input, message, Select, Space, Typography
} from "antd";
import locale from "antd/es/date-picker/locale/vi_VN";
import moment from "moment";
import "moment/locale/vi";
import React, { Component } from "react";
import { myCheckAuth } from "../../Services/authServices";
import { GetListCompany } from "../../Services/CompanyService";
import { GetVehicleRegisterList2View } from "../../Services/KhaiBaoService/khaiBaoService";
import HeaderPage from "../Common/HeaderPage";
import "./LichSuGiaoNhan.less";
import TableLichSuGiaoNhan from "./TableLichSuGiaoNhan";
const { RangePicker } = DatePicker;
const { Text } = Typography;
const { Option } = Select;

export class LichSuGiaoNhan extends Component {
  static displayName = LichSuGiaoNhan.name;

  // Check login
  componentWillMount() {
    myCheckAuth();
  }
  constructor(props) {
    super(props);
    this.state = {
      listVehicleRegister: [],
      loading: true,
      taiXeList: [],
      selectedRecord: {},
      driverSelected: "",
      total: 0,
      plants: [],
    };
  }

  searchListVehicleRegister(values) {
    console.log(values);
    var startDate = values.time[0].format("YYYY-MM-DD");
    var endDate = values.time[1].format("YYYY-MM-DD");

    // message.loading('Đang tìm kiếm')
    this.getListVehicleRegister(
      startDate,
      endDate,
      values.orderNumber,
      values.plant
    );
  }

  componentDidMount() {
    const from = moment().format("YYYY-MM-DD");
    const to = moment().format("YYYY-MM-DD");
    this.getListVehicleRegister(from, to, undefined);
    this.getNoiGioaNhan();
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

  render() {
    const optionsPlant = this.state.plants.map((plant) => (
      <Option key={plant.companyCode} value={plant.companyCode}>
        {plant.companyName}
      </Option>
    ));
    return (
      <React.Fragment>
        <HeaderPage
          title="Lịch sử giao nhận"
          description="Danh sách kết quả giao/nhận hàng"
        />
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
                  rules={[{ required: true, message: "Chưa nhập thônng tin" }]}
                >
                  <RangePicker
                    locale={locale}
                    placeholder={["Từ ngày", "Đến ngày"]}
                    format={"DD/MM/YYYY"}
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
        <TableLichSuGiaoNhan
          loading={this.state.loading}
          dataSource={this.state.listVehicleRegister}
        />
      </React.Fragment>
    );
  }

  async getListVehicleRegister(startDate, endDate, orderNumber, plant) {
    if (localStorage.getItem("type") === "Customer") {
    } else {
      this.setState({ loading: true });
      await GetVehicleRegisterList2View(
        startDate,
        endDate,
        orderNumber,
        plant,
        false
      ).then((objRespone) => {
        var totalTrongLuong = 0;
        if (objRespone.isSuccess === true) {
          var list = objRespone.data.map((record) => {
            record.key = record.item.vehicleRegisterMobileId;
            record.vehicleNumber = record.item.vehicleNumber;
            record.romooc = record.item.romooc;
            record.soDonHang = record.item.soDonHang;
            record.driverName = record.item.driverName;
            record.driverIdCard = record.item.driverIdCard;
            record.trongLuongGiaoDuKien = record.item.trongLuongGiaoDuKien;
            record.trongLuongGiaoThucTe = record.item.trongLuongGiaoThucTe;
            record.thoiGianToiThucTe = record.item.thoiGianToiThucTe;
            record.allowEdit = record.item.allowEdit;
            record.cungDuongCode = record.item.cungDuongCode;
            record.cungDuongName = record.item.cungDuongName;
            record.vatTu = record.detail.map((detail) => ({
              productCode: detail.productCode,
              productName: detail.productName,
            }));
            record.taiSanTheoXe = record.item.note;
            record.assets = record.item.assets;
            record.thoiGianToiDuKien = moment(record.item.thoiGianToiDuKien);
            record.scaleTicketCode =
              record.item.scaleTicketCode == null
                ? [""]
                : record.item.scaleTicketCode.split(", ");
            // record.scaleTicketCode = record.item.scaleTicketCode
            record.productCodeList = record.detail.map(
              (item) => item.productCode
            );
            record.productNameList = record.detail.map(
              (item) => item.productName
            );
            record.tiLeList = record.detail.map((item) => item.tiLe / 100);
            record.unitList = record.detail.map((item) => item.unit);
            // Đối với phiếu tách tải
            record.trongLuongTTList =
              record.detail.length == 1
                ? [record.item.trongLuongGiaoThucTe]
                : record.detail.map((item) => item.trongLuong);
            //
            record.tapChat = record.item.tapChat;
            record.chenhLech =
              record.item.trongLuongGiaoDuKien > 0
                ? Math.abs(
                    record.item.trongLuongGiaoThucTe -
                      record.item.trongLuongGiaoDuKien
                  )
                : null;
            record.tiLeChechLech =
              record.chenhLech / record.item.trongLuongGiaoDuKien;
            totalTrongLuong += record.item.trongLuongGiaoThucTe;

            return record;
          });
          var total = {
            key: "total",
            thoiGianToiDuKien: "Tổng cộng",
            trongLuongGiaoDuKien: 0,
            tiLeList: [0],
            trongLuongTTList: [0],
            chenhLech: 0,
            tapChat: 0,
            productNameList: [],
            productCodeList: [],
          };
          var tempTotal = 0;
          for (var i = 0; i < list.length; i++) {
            total.trongLuongGiaoDuKien += list[i].trongLuongGiaoDuKien;
            tempTotal += list[i].trongLuongTTList.reduce((total, num) => {
              return total + num;
            });
            // console.log( );
            total.chenhLech += list[i].chenhLech;
            total.tapChat += list[i].tapChat;
          }

          total.tiLeChechLech = total.chenhLech / total.trongLuongGiaoDuKien;
          total.trongLuongTTList = [tempTotal];
          console.log(total);

          list.push(total);

          this.setState({
            listVehicleRegister: list,
            total: totalTrongLuong,
          });
        } else {
          message.error(objRespone.err.msgString);
        }
        this.setState({ loading: false });
      });
    }
  }
}
