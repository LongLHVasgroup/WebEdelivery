import {
  Button,
  Card,
  Col,
  DatePicker,
  Divider,
  Form,
  message,
  Row,
  Select,
  Space,
  Spin,
  Typography
} from "antd";
import locale from "antd/es/date-picker/locale/vi_VN";
import { Chart } from "chart.js";
import "chartjs-plugin-datalabels";
import ChartDataLabels from "chartjs-plugin-datalabels";
import moment from "moment";
import "moment/locale/vi";
import React, { Component } from "react";
import { defaults } from "react-chartjs-2";
import { myCheckAuth } from "../../Services/authServices";
import { GetListCompany } from "../../Services/CompanyService";
import { GetReportTrongLuongChung } from "../../Services/ReportService";
import { GetDataReportChart } from "../../Services/Summary";
import "./index.less";
import ChartBar from "./Summary/ChartBar";
import ChartLine from "./Summary/ChartLine";
import ChartTitle from "./Summary/ChartTitle";
import TablePhanBoTL from "./Summary/TablePhanBoTL";
import TongTrongLuong from "./Summary/TongTL";

// Register the plugin to all charts:
defaults.font.fontFamily = "Glypha VO";
Chart.register(ChartDataLabels);

const { RangePicker } = DatePicker;
const { Text } = Typography;
const { Option } = Select;

const LATEST_PLANT = "latestPlant";
const PLANTS = "plants";

export class Summary extends Component {
  static displayName = Summary.name;

  formRef = React.createRef();

  // Check login
  componentWillMount() {
    myCheckAuth();
  }
  constructor(props) {
    super(props);
    this.state = {
      loadingTLChung: true,
      loadingChart: true,
      noiDia: 0,
      nhapKhau: 0,
      total: 0,
      typeTimeChartLine: "week",
      dataChartLine: {},
      // Total
      dataChartBarTotal: {},
      dataChartBarTotalTable: [],
      weightTotalChartBar: 0,
      // Nội đia
      dataChartBarNoiDia: {},
      dataChartBarNoiDiaTable: [],
      weightTotalNoiDia: 0,
      // Nhập khẩu
      dataChartBarNhapKhau: {},
      dataChartBarNhapKhauTable: [],
      weightTotalNhapKhau: 0,
      sourceType: 1,
      globalFromDate: moment().startOf("week"),
      globalToDate: moment().endOf("week"),
      weekActive: "primary", // active  mặc định là tuần
      monthActive: "dashed",
      yearActive: "dashed",
      plants: [],
      selectedPlant: undefined,
    };
  }

  componentDidMount() {
    const plant = this.getLatestSelectedPlant();
    this.setState({
      selectedPlant: plant,
    });

    this.getNoiGiaoNhan();

    // Lấy data lên thông tin trọng lượng
    this.getTrongLuongChung(
      moment().startOf("week").format("YYYY-MM-DD"),
      moment().endOf("week").format("YYYY-MM-DD"),
      plant
    );

    // Lấy dữ liệu biểu đồ Line
    this.getDataChart(
      "week",
      moment().startOf("week").format("YYYY-MM-DD"),
      moment().endOf("week").format("YYYY-MM-DD"),
      plant
    );
  }
  // lưu khi chuyển component
  componentWillUnmount() {
    localStorage.setItem(LATEST_PLANT, this.state.selectedPlant);
  }

  getLatestSelectedPlant() {
    // lấy lại lần chọn plant cuối cùng
    const latestPlant = localStorage.getItem(LATEST_PLANT);
    if (latestPlant) {
      return latestPlant;
    } else {
      // mặc đinh chọn 4000
      return "4000";
    }
  }

  changePlantHandler(plant) {
    this.setState({
      selectedPlant: plant,
    });

    console.log(plant);
    this.getTrongLuongChung(
      this.state.globalFromDate.format("YYYY-MM-DD"),
      this.state.globalToDate.format("YYYY-MM-DD"),
      plant
    );

    // Lấy dữ liệu biểu đồ Line
    this.getDataChart(
      this.state.typeTimeChartLine,
      this.state.globalFromDate.format("YYYY-MM-DD"),
      this.state.globalToDate.format("YYYY-MM-DD"),
      plant
    );
  }

  handleChangeSource(type) {
    this.setState({
      sourceType: type,
    });
  }

  handleGetDataChartBar(source) {
    // xử lý cho data theo total
    this.setState({
      dataChartBarTotal: this.mappingData2ChartBar(source),
      dataChartBarTotalTable: this.mappingData2ChartBarTable(source),
    });

    var noiDia = [];
    var nhapKhau = [];
    var weightTotal = 0;
    var weightTotalNoiDia = 0;
    var weightTotalNhapKhau = 0;
    for (var i = 0; i < source.length; i++) {
      weightTotal += source[i].weight;
      if (source[i].providerCode.charAt(0) == "1") {
        noiDia.push(source[i]);
        weightTotalNoiDia += source[i].weight;
      } else {
        nhapKhau.push(source[i]);
        weightTotalNhapKhau += source[i].weight;
      }
    }
    // Lấy trọng lượng tổng
    this.setState({
      weightTotalChartBar: weightTotal,
      weightTotalNhapKhau: weightTotalNhapKhau,
      weightTotalNoiDia: weightTotalNoiDia,
    });
    // xử lý cho data theo NoiDia
    this.setState({
      dataChartBarNoiDia: this.mappingData2ChartBar(noiDia),
      dataChartBarNoiDiaTable: this.mappingData2ChartBarTable(noiDia),
    });
    // xử lý data theo NhapKhau
    this.setState({
      dataChartBarNhapKhau: this.mappingData2ChartBar(nhapKhau),
      dataChartBarNhapKhauTable: this.mappingData2ChartBarTable(nhapKhau),
    });
  }
  mappingData2ChartLine(type, responseData) {
    responseData = this.modify2FullDate(
      type,
      this.state.globalFromDate,
      responseData
    );
    console.log(responseData);
    return {
      labels:
        type == "year"
          ? responseData.map((item) => moment(item.date).format("MM/YYYY"))
          : responseData.map((item) => moment(item.date).format("DD/MM")),
      datasets: [
        {
          label: "Trọng lượng sắt phế liệu nhập, kg",
          fill: false,
          lineTension: 0.1,
          backgroundColor: "rgba(255, 159, 64, 0.2)",
          borderColor: "#f26600",
          borderCapStyle: "butt",
          borderDash: [],
          borderWidth: 2,
          borderDashOffset: 0.0,
          borderJoinStyle: "miter",
          pointBorderColor: "#f26600",
          pointBackgroundColor: "#fff",
          pointBorderWidth: 1,
          pointHoverRadius: 5,
          pointHoverBackgroundColor: "#f26600",
          pointHoverBorderColor: "#f26600",
          pointHoverBorderWidth: 2,
          pointRadius: 1,
          pointHitRadius: 10,
          data: responseData.map((item) => item.weight),
        },
      ],
    };
  }

  genRamdomColor(size) {
    var colorArr = [];
    while (size > 0) {
      // colorArr.push('#' + Math.floor(Math.random() * 16777215).toString(16))
      colorArr.push("rgba(242, 102, 0, 0.7)");
      size--;
    }
    return colorArr;
  }

  modifyTitleCharBar(length) {
    var titleArr = [];
    while (length > 0) {
      titleArr.push(length);
      length--;
    }
    return titleArr.reverse();
  }

  mappingData2ChartBar(dataReponse) {
    return {
      title: {
        display: true,
        text: "Chart.js Bar Chart",
      },
      labels: this.modifyTitleCharBar(dataReponse.length),
      datasets: [
        {
          label: [""],
          data: dataReponse.map((item) => item.weight),
          backgroundColor: this.genRamdomColor(12),
          // borderColor: this.genRamdomColor(12),
          // borderWidth: 1,
        },
      ],
    };
  }
  mappingData2ChartBarTable(dataReponse) {
    var sumTotal = 0;
    for (var i = 0; i < dataReponse.length; i++) {
      sumTotal += dataReponse[i].weight;
    }
    return dataReponse.map((item, index) => ({
      providerName: item.providerName,
      value: item.weight,
      percent: item.weight / sumTotal,
      // stt: dataReponse.length -index,
      stt: index + 1,
    }));
  }

  modify2FullDate(typeRange, dateInRange, dataReponse) {
    var modifiedData = [];
    var startDate = moment();
    var endDate = moment();
    var position = 0;

    switch (typeRange) {
      case "day":
        startDate = this.state.globalFromDate;
        endDate = this.state.globalToDate;
        while (startDate <= endDate) {
          if (dataReponse[position] == undefined) {
            modifiedData.push({
              date: startDate.format("YYYY-MM-DDT00:00:00"),
              weight: 0,
            });
          } else if (
            startDate.format("DD/MM/YYYY") ==
            moment(dataReponse[position].date).format("DD/MM/YYYY")
          ) {
            modifiedData.push({
              date: dataReponse[position].date,
              weight: dataReponse[position].weight,
            });
            position++;
          } else {
            modifiedData.push({
              date: startDate.format("YYYY-MM-DDT00:00:00"),
              weight: 0,
            });
          }
          startDate = startDate.clone().add(1, "days");
        }
        break;
      case "week":
        startDate = moment(dateInRange).startOf("week");
        endDate = moment(dateInRange).endOf("week");

        while (startDate < endDate) {
          if (dataReponse[position] == undefined) {
            modifiedData.push({
              date: startDate.format("YYYY-MM-DDT00:00:00"),
              weight: 0,
            });
          } else if (
            startDate.format("DD/MM/YYYY") ==
            moment(dataReponse[position].date).format("DD/MM/YYYY")
          ) {
            modifiedData.push({
              date: dataReponse[position].date,
              weight: dataReponse[position].weight,
            });
            position++;
          } else {
            modifiedData.push({
              date: startDate.format("YYYY-MM-DDT00:00:00"),
              weight: 0,
            });
          }
          startDate = startDate.add(1, "days");
        }
        break;
      case "month":
        startDate = moment(dateInRange).startOf("month");
        endDate = moment(dateInRange).endOf("month");

        while (startDate < endDate) {
          if (dataReponse[position] == undefined) {
            modifiedData.push({
              date: startDate.format("YYYY-MM-DDT00:00:00"),
              weight: 0,
            });
          } else if (
            startDate.format("DD/MM/YYYY") ==
            moment(dataReponse[position].date).format("DD/MM/YYYY")
          ) {
            modifiedData.push({
              date: dataReponse[position].date,
              weight: dataReponse[position].weight,
            });
            position++;
          } else {
            modifiedData.push({
              date: startDate.format("YYYY-MM-DDT00:00:00"),
              weight: 0,
            });
          }
          startDate = startDate.add(1, "days");
        }
        break;

      case "year":
        startDate = moment(dateInRange).startOf("year");
        endDate = moment(dateInRange).endOf("year");
        for (var i = 0; i < 12; i++) {
          var weightTotal = 0;

          while (
            dataReponse[position] !== undefined &&
            moment(dataReponse[position].date).month() <= i
          ) {
            console.log(moment(dataReponse[position].date).month());
            weightTotal += dataReponse[position].weight;
            position++;
          }
          modifiedData.push({
            date: startDate.format("YYYY-MM-DDT00:00:00"),
            weight: weightTotal,
          });
          startDate = startDate.add(1, "month");
        }
        break;
      default:
        break;
    }
    return modifiedData;
  }

  // thay đổi thời gian global
  handleChangeGlobalTime(typeChange, value) {
    var startDate = moment().format("YYYY-MM-DD");
    var endDate = moment().format("YYYY-MM-DD");
    switch (typeChange) {
      case "pick":
        if (!value) return;
        startDate = value[0].format("YYYY-MM-DD");
        endDate = value[1].format("YYYY-MM-DD");
        this.getDataChart("day", startDate, endDate);
        this.setState({
          typeTimeChartLine: "day",
          globalFromDate: value[0],
          globalToDate: value[1],
        });
        break;
      case "tab":
        switch (value) {
          case "week":
            this.formRef.current.setFieldsValue({
              time: [moment().startOf("week"), moment().endOf("week")],
            });
            startDate = moment().startOf("week").format("YYYY-MM-DD");
            endDate = moment().endOf("week").format("YYYY-MM-DD");
            this.getDataChart("week", startDate, endDate);
            this.setState({
              typeTimeChartLine: "week",
              globalFromDate: moment().startOf("week"),
              globalToDate: moment().endOf("week"),
              weekActive: "primary", // không tô màu lên các nút nữa
              monthActive: "dashed",
              yearActive: "dashed",
            });

            break;

          case "month":
            this.formRef.current.setFieldsValue({
              time: [moment().startOf("month"), moment().endOf("month")],
            });
            startDate = moment().startOf("month").format("YYYY-MM-DD");
            endDate = moment().endOf("month").format("YYYY-MM-DD");
            this.getDataChart("month", startDate, endDate);
            this.setState({
              typeTimeChartLine: "month",
              globalFromDate: moment().startOf("month"),
              globalToDate: moment().endOf("month"),
              weekActive: "dashed", // không tô màu lên các nút nữa
              monthActive: "primary",
              yearActive: "dashed",
            });
            break;
          case "year":
            this.formRef.current.setFieldsValue({
              time: [moment().startOf("year"), moment().endOf("year")],
            });
            startDate = moment().startOf("year").format("YYYY-MM-DD");
            endDate = moment().endOf("year").format("YYYY-MM-DD");
            this.getDataChart("year", startDate, endDate);
            this.setState({
              typeTimeChartLine: "year",
              globalFromDate: moment().startOf("year"),
              globalToDate: moment().endOf("year"),
              weekActive: "dashed", // không tô màu lên các nút nữa
              monthActive: "dashed",
              yearActive: "primary",
            });
            break;
          default:
            break;
        }

        break;
    }
    // Lấy tổng trọng lượng
    this.getTrongLuongChung(startDate, endDate, this.state.selectedPlant);
  }

  render() {
    const dataChartBar =
      this.state.sourceType == "1"
        ? this.state.dataChartBarTotal
        : this.state.sourceType == "2"
        ? this.state.dataChartBarNoiDia
        : this.state.dataChartBarNhapKhau;

    const dataTable =
      this.state.sourceType == "1"
        ? this.state.dataChartBarTotalTable
        : this.state.sourceType == "2"
        ? this.state.dataChartBarNoiDiaTable
        : this.state.dataChartBarNhapKhauTable;

    const summaryTotal =
      this.state.sourceType == "1"
        ? this.state.weightTotalChartBar
        : this.state.sourceType == "2"
        ? this.state.weightTotalNoiDia
        : this.state.weightTotalNhapKhau;

    const summaryPercent =
      this.state.sourceType == "1"
        ? 1
        : this.state.sourceType == "2"
        ? this.state.weightTotalNoiDia / this.state.weightTotalChartBar
        : this.state.weightTotalNhapKhau / this.state.weightTotalChartBar;

    const optionsPlant = this.state.plants.map((p) => (
      <Option key={p.companyCode} value={p.companyCode}>
        {p.companyName}
      </Option>
    ));

    return (
      <div>
        {/* <h2 id="tabelLabel" >Tổng quan</h2> */}
        <br />

        <div>
          <div class="d-flex">
            <div class="mr-auto p-2">
              <Space>
                <h5 style={{ margin: "0px" }}>Đơn vị:</h5>
                <Select
                  size="large"
                  defaultValue={this.getLatestSelectedPlant()}
                  bordered={false}
                  onChange={(value) => this.changePlantHandler(value)}
                >
                  {optionsPlant}
                </Select>
              </Space>
            </div>
            <div class="p-2" style={{ alignSelf: "center" }}>
              <Space>
                <Text>Chọn thời gian xem báo cáo</Text>
                <Divider className="divider" type="vertical" />
                <Button
                  onClick={() => this.handleChangeGlobalTime("tab", "week")}
                  type="text"
                  // style={{ color: this.state.weekActive }}
                  shape="round"
                  type={this.state.weekActive}
                >
                  Tuần
                </Button>
                <Button
                  onClick={() => this.handleChangeGlobalTime("tab", "month")}
                  type="text"
                  // style={{ color: this.state.monthActive }}
                  shape="round"
                  type={this.state.monthActive}
                >
                  Tháng
                </Button>
                <Button
                  onClick={() => this.handleChangeGlobalTime("tab", "year")}
                  type="text"
                  // style={{ color: this.state.yearActive }}

                  shape="round"
                  type={this.state.yearActive}
                >
                  Năm
                </Button>
                <Divider type="vertical" className="divider" />{" "}
                <Form
                  ref={this.formRef}
                  className="my-range-picker"
                  style={{ textAlignLast: "start" }}
                  initialValues={{
                    time: [this.state.globalFromDate, this.state.globalToDate],
                  }}
                >
                  <Form.Item name="time" className="my-range-picker">
                    <RangePicker
                      onChange={(value) => {
                        this.handleChangeGlobalTime("pick", value);
                      }}
                      locale={locale}
                      format="DD/MM/YYYY"
                      bordered={false}
                      placeholder={["Từ ngày", "Đến ngày"]}
                    />
                  </Form.Item>
                </Form>
              </Space>
            </div>
          </div>

          <Divider style={{ marginTop: "0px", marginBottom: "8px" }} />
          <Row gutter={[24, 16]}>
            <Col span={6} xs={24} lg={8} xl={6}>
              <TongTrongLuong
                loading={this.state.loadingTLChung}
                noiDia={this.state.noiDia}
                nhapKhau={this.state.nhapKhau}
                total={this.state.total}
              />
            </Col>
            <Col span={18} xs={24} lg={16} xl={18}>
              <ChartLine
                loading={this.state.loadingChart}
                data={this.state.dataChartLine}
              />
            </Col>
            <Col span={24} xs={24} xl={24}>
              <Card className="card-ncc">
                <Row gutter={24}>
                  <Col
                    span={12}
                    xs={24}
                    xl={12}
                    style={{ alignSelf: "center" }}
                  >
                    <ChartTitle title="Phân bổ trọng lượng đã nhập theo nhà cung cấp" />
                  </Col>
                  <Col
                    span={12}
                    xs={24}
                    xl={12}
                    style={{ textAlignLast: "end" }}
                  >
                    <Space>
                      Chọn xem theo nguồn hàng:
                      <Select
                        defaultValue="1"
                        bordered={false}
                        style={{ width: 140 }}
                        onChange={(value) => this.handleChangeSource(value)}
                      >
                        <Option value="1">Tổng</Option>
                        <Option value="2">Trong nước</Option>
                        <Option value="3">Nước ngoài</Option>
                      </Select>
                      Đơn vị tính: kg
                    </Space>
                  </Col>
                  <Divider style={{ margin: "0px", marginTop: "16px" }} />
                </Row>

                <Row gutter={24}>
                  <Col span={13} xs={24} xl={13}>
                    <ChartBar
                      loading={this.state.loadingChart}
                      data={dataChartBar}
                    />
                  </Col>
                  <Col span={11} xs={24} xl={11}>
                    <br />
                    <Spin spinning={this.state.loadingChart}>
                      <TablePhanBoTL
                        data={dataTable}
                        total={summaryTotal}
                        percent={summaryPercent}
                      />
                    </Spin>
                  </Col>
                </Row>
              </Card>
            </Col>
          </Row>
        </div>
      </div>
    );
  }

  // Lấy trọng lượng chung
  async getTrongLuongChung(startDate, endDate, plant) {
    this.setState({ loadingTLChung: true });
    await GetReportTrongLuongChung(startDate, endDate, plant, false).then(
      (objRespone) => {
        if (objRespone.isSuccess === true) {
          this.setState({
            noiDia: objRespone.item.noiDia[0].noiDia,
            nhapKhau: objRespone.item.nhapKhau[0].nhapKhau,
            total: objRespone.item.tongCong[0].tongCong,
            // plant: objRespone.item.plant[0].plantCode,
          });
        } else {
          message.error(objRespone.err.msgString);
        }
        this.setState({ loadingTLChung: false });
      }
    );
  }
  async getNoiGiaoNhan() {
    await GetListCompany().then((objRespone) => {
      if (objRespone.isSuccess === true) {
        this.setState({
          plants: objRespone.data,
        });
        localStorage.setItem(PLANTS, JSON.stringify(objRespone.data));
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  }
  // Lấy dữu liệu cho 2 biểu đồ Line và Bar
  async getDataChart(type, startDate, endDate, plant) {
    if (!plant) {
      plant = this.state.selectedPlant;
    }

    this.setState({ loadingChart: true });
    await GetDataReportChart(startDate, endDate, plant, false).then(
      (objRespone) => {
        if (objRespone.isSuccess === true) {
          if (plant !== objRespone.item.plant[0].plantCode) {
            return;
          }
          // Chart Line
          this.setState({
            dataChartLine: this.mappingData2ChartLine(
              type,
              objRespone.item.byDate.reverse()
            ),
          });

          this.handleGetDataChartBar(
            objRespone.item.byProvider.sort(function (a, b) {
              return b.weight - a.weight;
            })
          );
        } else {
          message.error(objRespone.err.msgString);
        }
        this.setState({ loadingChart: false });
      }
    );
  }
}
