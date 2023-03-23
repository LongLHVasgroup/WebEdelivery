import {
  Button,
  Card,
  Col,
  Divider,
  Form,
  message,
  Row,
  Select,
  Spin,
  Table,
  Typography,
} from "antd";
import moment from "moment";
import "moment/locale/vi";
import React, { useEffect, useState } from "react";
import { Line } from "react-chartjs-2";
import { LATEST_PLANT } from "../../Constant";
import { GetListCompany } from "../../Services/CompanyService";
import {
  GetReportChiTietTienDoGiaoHang,
  GetReportVanChuyen,
} from "../../Services/ReportService";
import HeaderPage from "../Common/HeaderPage";
import { LatestPlant } from "../Common/LatestSelectedPlant";
import TextNumber, { FormatedNumber } from "../Common/TextNumber";
import TextPercent from "../Common/TextPercent";
const { Text } = Typography;
const { Option } = Select;
//
/**
 * Báo cáo trạng thái của các đơn hàng đang active
 * Chi tiết mỗi ngày đơn hàng đó giao bao nhiêu
 */

const TinhHinhGiaoHang = (props) => {
  const [dataSource, setDataSource] = useState([]);
  const [dataSourceDetail, setDataSourceDetail] = useState([]);
  const [dataChart, setDataChart] = useState({});
  const [isLoading, setIsLoading] = useState(true);
  const [isLoadingDetail, setIsLoadingDetail] = useState(false);
  const [selectedPlant, setSelectedPlant] = useState(LatestPlant());
  const [plants, setPlants] = useState([]);

  // didMount
  useEffect(() => {
    getDataReport(selectedPlant);
    getDonVi();
  }, []);

  useEffect(() => {
    return () => {
      localStorage.setItem(LATEST_PLANT, selectedPlant);
    };
  }, [selectedPlant]);

  // Search dữ liệu
  function handleSearch(formValue) {
    setSelectedPlant(formValue.plant);
    getDataReport(formValue.plant);
  }

  // Người dùng bấm xem chi tiết
  function handelViewDetail(poNumber) {
    setIsLoadingDetail(true);
    getDetailTranferByPO(poNumber, selectedPlant);
  }

  function mappingDataChart(originalData) {
    var tempData = [];
    if (originalData.length > 1) {
      for (var i = 0; i < originalData.length - 1; i++) {
        // Khoản cách giữ 2 line lớn hơn 1 ngày
        tempData.push(originalData[i]);
        var diffDate = moment(originalData[i + 1].inHour).diff(
          moment(originalData[i].inHour),
          "days"
        );
        if (diffDate > 1) {
          var tmpDate = moment(originalData[i].inHour);
          while (diffDate > 1) {
            tmpDate = tmpDate.add(1, "d");
            tempData.push({
              inHour: tmpDate.format(),
              tongTrongLuong: 0,
            });
            diffDate--;
          }
        }
      }
      tempData.push(originalData[i]);
    } else tempData.push(originalData[0]);

    var labels = tempData.map((item) => moment(item.inHour).format("DD/MM"));
    var data = tempData.map((item) => item.tongTrongLuong);
    var dataChart = {
      labels: labels,
      datasets: [
        {
          label: "Trọng lượng giao theo ngày",
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
          pointBorderWidth: 2,
          pointHoverRadius: 5,
          pointHoverBackgroundColor: "#f26600",
          pointHoverBorderColor: "#f26600",
          pointHoverBorderWidth: 2,
          pointRadius: 1,
          pointHitRadius: 10,
          data: data,
        },
      ],
    };
    setDataChart(dataChart);
  }

  const getDataReport = async (plant) => {
    setIsLoading(true);
    setDataSource([]);
    setDataChart({});
    setDataSourceDetail([]);
    await GetReportVanChuyen(plant, false).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setDataSource(objRespone.item);
      } else {
        message.error(objRespone.err.msgString);
      }
      setIsLoading(false);
    });
  };
  // GetReportChiTietTienDoGiaoHang

  // Lấy chi tiết ngìao hàng theo ngày của po
  const getDetailTranferByPO = async (ponumber, plant) => {
    // this.setState({ loading: true })
    await GetReportChiTietTienDoGiaoHang(ponumber, plant, false).then(
      (objRespone) => {
        if (objRespone.isSuccess === true) {
          setDataSourceDetail(objRespone.item);
          mappingDataChart(objRespone.item);
        } else {
          message.error(objRespone.err.msgString);
          mappingDataChart([]);
        }
        setIsLoadingDetail(false);
      }
    );
  };

  const getDonVi = async () => {
    await GetListCompany().then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setPlants(objRespone.data);
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  };

  const options = plants.map((p) => (
    <Option key={p.companyCode} value={p.companyCode}>
      {p.companyName}
    </Option>
  ));

  const optionsLine = {
    scales: {
      yAxes: {
        ticks: {
          beginAtZero: true,
        },
        ticks: {
          callback: function (value, index, values) {
            return FormatedNumber(value);
          },
        },
      },
      xAxes: [
        {
          gridLines: {
            display: false,
          },
          ticks: {
            fontFamily: "Glypha VO",
          },
        },
      ],
    },
    plugins: {
      legend: {
        labels: {
          // This more specific font property overrides the global property
          font: {
            family: "Glypha VO",
          },
        },
        title: {
          display: false,
        },
      },
      tooltip: {
        enabled: true,
        // mode: 'single',
        callbacks: {
          label: function (context) {
            var label = context.dataset.label || "";

            if (label) {
              label += ": ";
            }
            if (context.parsed.y !== null) {
              label +=
                new Intl.NumberFormat("de-DE").format(context.parsed.y) + " kg";
            }
            return label;
          },
        },
      },
      datalabels: {
        color: "#666",
        display: "auto", // nếu đủ khoản cách thì mới hiện số
        anchor: "end",
        align: "end",
        offset: -1,
        labels: {
            title: {
                font: {
                    family: "Glypha VO",
                },
            },
        },
        formatter: function (value, context) {
            if (value != 0) return FormatedNumber(value);
            else return "";
        },
      },
    },
  };

  const columns = [
    {
      title: "Nhà cung cấp",
      dataIndex: "providerName",
      key: "providerName",
    },
    {
      title: "Mã đơn hàng",
      dataIndex: "poNumber",
      key: "poNumber",
    },
    {
      title: "TL đơn hàng",
      dataIndex: "qtyTotal",
      key: "qtyTotal",
      align: "right",
      render: (text) => <TextNumber value={text} />,
    },
    {
      title: "Số chuyến",
      dataIndex: "soChuyen",
      key: "soChuyen",
      align: "right",
      render: (text) => <TextNumber value={text} />,
    },

    {
      title: "TL đã giao",
      dataIndex: "trongLuong",
      key: "trongLuong",
      align: "right",
      render: (text) => <TextNumber value={text} />,
      // width: '6%',
    },

    {
      title: "TL TB/Chuyến",
      dataIndex: "trongLuongTrungBinh",
      key: "trongLuongTrungBinh",
      align: "right",
      render: (text) => <TextNumber value={text} />,
      // width: '6%',
    },

    {
      title: "Tỷ lệ hoàn thành",
      dataIndex: "tiLeHoanThanh",
      key: "tiLeHoanThanh",
      align: "right",
      render: (text, row) => (
        <TextPercent value={row.trongLuong / row.qtyTotal} />
      ),
    },
    {
      title: "Chi tiết",
      dataIndex: "action",
      key: "action",
      align: "center",
      render: (text, record) => (
        <a
          key="view"
          onClick={() => {
            handelViewDetail(record.poNumber);
          }}
        >
          <Button size="small">Xem</Button>
        </a>
      ),
    },
  ];
  const columnsDetail = [
    {
      title: "Ngày vận chuyển",
      dataIndex: "inHour",
      key: "inHour",
      render: (text) => <>{moment(text).format("DD/MM/YYYY")}</>,
    },
    {
      title: "Trọng lượng giao",
      dataIndex: "tongTrongLuong",
      key: "tongTrongLuong",
      align: "right",
      render: (text) => <TextNumber value={text} />,
    },
    {
      title: "Số lượng xe",
      dataIndex: "numberOrTrans",
      key: "numberOrTrans",
      align: "right",
      render: (text) => <TextNumber value={text} />,
    },
    {
      title: "TL trung bình",
      dataIndex: "trongLuongTrungBinh",
      key: "trongLuongTrungBinh",
      align: "right",
      render: (text) => <TextNumber value={text} />,
    },
  ];

  return (
    <React.Fragment>
      <HeaderPage
        title="Tình hình giao hàng"
        description="Báo cáo tình hình giao hàng của các đơn hàng đang kích hoạt theo từng ngày"
      />
      <Form
        name="advanced_search"
        className="ant-advanced-search-form"
        initialValues={{
          plant: selectedPlant,
        }}
        onFinish={(value) => handleSearch(value)}
      >
        <Row gutter={24}>
          <Col span={8}>
            <Form.Item
              labelAlign="left"
              name="plant"
              label="Đơn vị"
              rules={[{ required: true, message: "Chưa chọn đơn vị" }]}
            >
              <Select>{options}</Select>
            </Form.Item>
          </Col>
          <Col span={5}>
            <Button type="primary" htmlType="submit" loading={isLoading}>
              {" "}
              Tìm kiếm{" "}
            </Button>
          </Col>
          <Col
            span={11}
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

      {/* <Text className="text-trong-luong" style={{ paddingBottom: "9px" }}>
        Trọng lượng, kg
      </Text> */}

      <Table
        columns={columns}
        className="my-table-detail-register"
        loading={isLoading}
        bordered
        pagination={{ showSizeChanger: true }}
        locale={{ emptyText: "Không có thông tin" }}
        dataSource={dataSource}
      />

      <Divider />
      <h5>Thông tin giao hàng của đơn hàng</h5>
      <Row gutter={[32, 16]}>
        <Col span={10} xs={24} xl={10}>
          <Card>
            {isLoadingDetail ? (
              <Spin>
                <Table
                  columns={columnsDetail}
                  className="my-table-detail-register"
                  pagination={false}
                  locale={{ emptyText: "Không có thông tin" }}
                  dataSource={dataSourceDetail}
                />
              </Spin>
            ) : (
              <Table
                columns={columnsDetail}
                className="my-table-detail-register"
                pagination={false}
                locale={{ emptyText: "Không có thông tin" }}
                dataSource={dataSourceDetail}
              />
            )}
          </Card>
        </Col>
        <Col span={14} xs={24} xl={14}>
          <Card>
            {isLoadingDetail ? (
              <Spin>
                <Line
                  //   ref="chart"
                  data={dataChart}
                  options={optionsLine}
                  height={100}
                />
              </Spin>
            ) : (
              <Line
                // ref="chart"
                data={dataChart}
                options={optionsLine}
                height={100}
              />
            )}
          </Card>
        </Col>
      </Row>
    </React.Fragment>
  );
};
export default TinhHinhGiaoHang;
