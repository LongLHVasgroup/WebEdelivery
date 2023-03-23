import {
  Button, Col, DatePicker, Space,
  Form,
  message, Row, Select, Table, Typography
} from "antd";
import locale from "antd/es/date-picker/locale/vi_VN";
import moment from "moment";
import "moment/locale/vi";
import React, { Component } from "react";
import { LATEST_PLANT } from "../../Constant";
import { GetListCompany } from "../../Services/CompanyService";
import { GetReportTinhHinhDonHangDieuPhoi } from "../../Services/ReportService";
import HeaderPage from "../Common/HeaderPage";
import { LatestPlant } from "../Common/LatestSelectedPlant";
import TextNumber from "../Common/TextNumber";
import TextPercent from "../Common/TextPercent";
import { ExportCSV } from "../ExportFile/ExportCSV";
const { Text } = Typography;
const { Option } = Select;
// const layoutSearch = {
//   labelCol: {
//     span: 9,
//   },
//   wrapperCol: {
//     span: 15,
//   },
// };

// Báo cáo tình hình vận chuyển-> liệt kê dữ liệu theo từng ngày

export class TinhHinhDieuPhoi extends Component {
  static displayName = TinhHinhDieuPhoi.name;

  // Check login
  // componentWillMount() {
  //     myCheckAuth();
  // }
  constructor(props) {
    super(props);
    this.state = {
      dataSource: [],
      dataExport: [],
      loading: true,
      plants: [],
      selectedPlant: LatestPlant(),
      loading: true,
    };
  }


  componentDidMount() {
    this.getDonVi();

    this.getDataReport("", "", moment().format("YYYY-MM-DD"), this.state.selectedPlant);
  }

  componentWillUnmount() {
    localStorage.setItem(LATEST_PLANT, this.state.selectedPlant);
  }

  // Search dữ liệu
  handleSearch(formValue) {
    console.log(formValue);
    this.setState({
      selectedPlant: formValue.plant,
    });
    this.getDataReport(
      "",
      "",
      formValue.fromDate.format("YYYY-MM-DD"),
      formValue.plant
    );
  }

  render() {
    const columns = [
      {
        title: "Mã đơn hàng",
        dataIndex: "orderNumber",
        key: "orderNumber",
      },
      {
        title: "Nhà cung cấp",
        dataIndex: "providerName",
        key: "providerName",
      },
      {
        title: "Đơn vị vận chuyển",
        dataIndex: "dvvc",
        key: "dvvc",
      },
      {
        title: "TL điều phối/ Số cont",
        dataIndex: "soLuong",
        key: "soLuong",
        align: "right",
        render: (text, row) =>
          text !== 0 ? (
            <TextNumber value={text} />
          ) : (
            <TextNumber value={row.soLuongCont} />
          ),
        // width: '6%',
      },
      {
        title: "Đã chuyển",
        dataIndex: "daVanChuyen",
        key: "daVanChuyen",
        align: "right",
        render: (text) => <TextNumber value={text} />,
        // width: '6%',
      },
      {
        title: "Số chuyến",
        dataIndex: "numberOfTrans",
        key: "numberOfTrans",
        align: "right",
        render: (text) => <TextNumber value={text} />,
        // width: '6%',
      },

      {
        title: "TL trung bình",
        dataIndex: "trongLuongTrungBinh",
        key: "trongLuongTrungBinh",
        align: "right",
        render: (text) => <TextNumber value={text} />,
        // width: '6%',
      },

      {
        title: "Tỷ lệ hoàn thành",
        dataIndex: "tyLeHoanThanh",
        key: "tyLeHoanThanh",
        align: "right",
        render: (text, row) =>
          row.soLuong === 0 ? (
            <TextPercent value={row.numberOfTrans / row.soLuongCont} />
          ) : (
            <TextPercent value={row.daVanChuyen / row.soLuong} />
          ),
        // width: '6%',
      },

      {
        title: "Cung đường",
        dataIndex: "cungDuongName",
        key: "cungDuongName",
      },
    ];

    const option = this.state.plants.map((p) => (
      <Option key={p.companyCode} value={p.companyCode}>
        {p.companyName}
      </Option>
    ));

    return (
      <React.Fragment>
        <HeaderPage
          title="Tình hình điều phối đơn hàng"
          description="Báo cáo tình hình vận chuyển của các đơn hàng được điều phối"
        />

        <Form
          // form={form}
          name="advanced_search"
          className="ant-advanced-search-form"
          //   {...layoutSearch}
          initialValues={{
            fromDate: moment(),
            toDate: moment(),
            plant: this.state.selectedPlant,
          }}
          onFinish={(value) => this.handleSearch(value)}
        // onFinish={onFinish}
        >
          {/* <Row gutter={24}>{getFields()}</Row> */}
          <Row gutter={24}>
            <Col span={5}>
              <Form.Item
                labelAlign="left"
                name="fromDate"
                label="Từ ngày"
                rules={[{ required: true, message: "Chưa chọn ngày" }]}
              >
                <DatePicker
                  style={{ width: "100%" }}
                  locale={locale}
                  placeholder=""
                  format={"DD/MM/YYYY"}
                />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item
                labelAlign="left"
                name="plant"
                label="Đơn vị"
                rules={[{ required: true, message: "Chưa chọn đơn vị" }]}
              >
                <Select>{option}</Select>
              </Form.Item>
            </Col>
            <Col span={5}>
              <Space style={{ width: '100%' }}>
                <Button type="primary" htmlType="submit" loading={this.state.loading}>
                  {" "}
                  Tìm kiếm{" "}
                </Button>
                <ExportCSV csvData={this.state.dataExport} fileName="Lịch sử điều phối" />
              </Space>
            </Col>
            {/* <Col span={6}></Col> */}
            <Col
              span={6}
              style={{
                alignSelf: "flex-end",
                paddingRight: "20px",
                paddingBottom: "8px",
              }}
            >
              <Text className="text-trong-luong">Trọng lượng, kg</Text>
            </Col>
          </Row>
        </Form>


        <Table
          columns={columns}
          // dataSource={this.state.listVehicleRegister}
          className="my-table-detail-register"
          loading={this.state.loading}
          bordered
          pagination={{ showSizeChanger: true }}
          locale={{ emptyText: "Không có thông tin" }}
          dataSource={this.state.dataSource}
        />


      </React.Fragment>
    );
  }
  async getDataReport(pageSize, pageNumber, fromDate, plant) {
    this.setState({ loading: true });
    console.log(plant);
    await GetReportTinhHinhDonHangDieuPhoi(fromDate, plant, false).then(
      (objRespone) => {
        if (objRespone.isSuccess === true) {
          this.setState({
            dataSource: objRespone.item,
          });

          var data2Export = []
          try {
            objRespone.item.forEach(element => {
              data2Export.push({
                MaDonHang: element.orderNumber,
                NhaCungCap: element.providerName,
                DonViVanChuyen: element.dvvc,
                TrongLuongDieuPhoi: element.soLuong,
                SoContDieuPhoi: element.soLuongCont,
                TLDaChuyen: element.daVanChuyen,
                SoChuyen: element.numberOfTrans,
                TLTrungBinh: element.trongLuongTrungBinh,
                TiLeHoanThanh: element.soLuong === 0 ? element.numberOfTrans / element.soLuongCont : element.daVanChuyen / element.soLuong,
                CungDuong: element.cungDuongName
              })
            });

            this.setState({
              dataExport: data2Export
            })
          } catch {

          }

        } else {
          message.error(objRespone.err.msgString);
        }
        this.setState({ loading: false });
      }
    );
  }

  async getDonVi() {
    await GetListCompany().then((objRespone) => {
      if (objRespone.isSuccess === true) {
        this.setState({
          plants: objRespone.data,
        });
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  }
}
