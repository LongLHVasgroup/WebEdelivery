import {
    message, Table
} from "antd";
import moment from "moment";
import "moment/locale/vi";
import React, { Component } from "react";
import { GetListCompany } from "../../Services/CompanyService";
import { GetReportGiaoNhan } from "../../Services/ReportService";
import FormSearchReportNhapHang from "../Common/FormSearchReportNhapHang";
import HeaderPage from "../Common/HeaderPage";
import TextNumber from "../Common/TextNumber";
import TextPercent from "../Common/TextPercent";
const LATEST_PLANT = "latestPlant";
const DEFAULT_PLANT = "4000";
export class NhapHang extends Component {
  static displayName = NhapHang.name;

  constructor(props) {
    super(props);
    this.state = {
      dataSource: [],
      loading: true,
      plants: [],
    };
  }
  // Check login
  // componentWillMount() {
  //     myCheckAuth();
  // }
  componentDidMount() {
    var p = this.getLatestPlant();
    var fromDate = moment().format("YYYY-MM-DD");
    var toDate = moment().format("YYYY-MM-DD");

    this.getDonVi();
    this.getDataReport(0, 0, fromDate, toDate, p);
  }

  getLatestPlant() {
    var plant = localStorage.getItem(LATEST_PLANT) || null;
    if (!plant) {
      plant = DEFAULT_PLANT;
    }
    return plant;
  }

  handleSearch(formValue) {
      console.log(formValue)
    this.getDataReport(
      0,
      0,
      formValue.fromDate.format("YYYY-MM-DD"),
      formValue.toDate.format("YYYY-MM-DD"),
      formValue.plant
    );
  }

  render() {
    const columns = [
      {
        title: "Nhà cung cấp",
        dataIndex: "providerName",
        key: "providerName",
        // render: (text, row) => text.slice(0, 10),
        // render: (text, row) => <>{row.key === 'total' ? <Title level={5}>{text}</Title> : row.thoiGianToiDuKien.format('DD/MM/YYYY')}</>,
      },
      {
        title: "Mã đơn hàng",
        dataIndex: "poNumber",
        key: "poNumber",
        // render: (text, row) => text.slice(0, 10),
        // render: (text, row) => <>{row.key === 'total' ? <Title level={5}>{text}</Title> : row.thoiGianToiDuKien.format('DD/MM/YYYY')}</>,
        width: "10%",
      },
      {
        title: "Khai báo",
        dataIndex: "khaiBao",
        key: "khaiBao",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "7%",
      },
      {
        title: "Thực tế",
        dataIndex: "canThucTe",
        key: "canThucTe",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "7%",
      },

      {
        title: "Chêch lệch",
        dataIndex: "chenhLech",
        key: "chenhLech",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "7%",
      },

      {
        title: "Tỷ lệ hàng",
        dataIndex: "tiLe",
        key: "tiLe",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextPercent value={text} />,

        width: "7%",
      },

      {
        title: "Số lượng đơn hàng",
        dataIndex: "qtyTotal",
        key: "qtyTotal",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "8%",
      },
      {
        title: "Số lượng đã giao",
        dataIndex: "trongLuongDaNhap",
        key: "trongLuongDaNhap",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "7%",
      },
      {
        title: "Còn lại",
        dataIndex: "trongLuongConLai",
        key: "trongLuongConLai",
        align: "right",
        render: (text) => <TextNumber value={text} />,
        width: "7%",
      },
      {
        title: "Tỷ lệ hoàn thành",
        dataIndex: "tyLeHoanThanh",
        key: "tyLeHoanThanh",
        align: "right",
        render: (text) => <TextPercent value={text} />,
        width: "7%",
      },
    ];

    const columnsSub = [
      {
        title: "Nhà cung cấp",
        dataIndex: "providerName",
        key: "providerName",
        // render: (text, row) => text.slice(0, 10),
        // render: (text, row) => <>{row.key === 'total' ? <Title level={5}>{text}</Title> : row.thoiGianToiDuKien.format('DD/MM/YYYY')}</>,
      },
      {
        title: "Mã đơn hàng",
        dataIndex: "poNumber",
        key: "poNumber",
        // render: (text, row) => text.slice(0, 10),
        // render: (text, row) => <>{row.key === 'total' ? <Title level={5}>{text}</Title> : row.thoiGianToiDuKien.format('DD/MM/YYYY')}</>,
        width: "10.3%",
      },
      {
        title: "Khai báo",
        dataIndex: "khaiBao",
        key: "khaiBao",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "7.2%",
      },
      {
        title: "Thực tế",
        dataIndex: "canThucTe",
        key: "canThucTe",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "7.3%",
      },

      {
        title: "Chêch lệch",
        dataIndex: "chenhLech",
        key: "chenhLech",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "7.3%",
      },

      {
        title: "Tỷ lệ hàng",
        dataIndex: "tiLe",
        key: "tiLe",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextPercent value={text} />,

        width: "7.5%",
      },

      {
        title: "Số lượng đơn hàng",
        dataIndex: "qtyTotal",
        key: "qtyTotal",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "8.2%",
      },
      {
        title: "Số lượng đã giao",
        dataIndex: "trongLuongDaNhap",
        key: "trongLuongDaNhap",
        align: "right",
        // render: (text, row) => text.slice(0, 10),
        render: (text) => <TextNumber value={text} />,
        width: "7.3%",
      },
      {
        title: "Còn lại",
        dataIndex: "trongLuongConLai",
        key: "trongLuongConLai",
        align: "right",
        render: (text) => <TextNumber value={text} />,
        width: "7.3%",
      },
      {
        title: "Tỷ lệ hoàn thành",
        dataIndex: "tyLeHoanThanh",
        key: "tyLeHoanThanh",
        align: "right",
        render: (text) => <TextPercent value={text} />,
        width: "7.2%",
      },
    ];

    return (
      <div>
        <HeaderPage
          title="Tổng hợp nhập sắt phế liệu"
          description="Báo cáo tình hình nhập sắt phế liệu theo nhà cung cấp và theo đơn hàng"
        />
        <FormSearchReportNhapHang
          from={moment()}
          to={moment()}
          defaultPlant={this.getLatestPlant()}
          plants={this.state.plants}
          onFinish={values => this.handleSearch(values)}
        />

        <Table
          columns={columns}
          // dataSource={this.state.listVehicleRegister}
          className="my-table-report-tong-hop"
          loading={this.state.loading}
          bordered
          pagination={{ showSizeChanger: true }}
          locale={{ emptyText: "Không có thông tin" }}
          dataSource={this.state.dataSource}
          expandable={{
            expandedRowRender: (record) => (
              <>
                <Table
                  columns={columnsSub}
                  // dataSource={this.state.listVehicleRegister}
                  className="my-table-report-tong-hop-item"
                  // loading={this.state.loading}
                  // style={}
                  bordered
                  showHeader={false}
                  pagination={false}
                  dataSource={record.items}
                />
              </>
            ),
            rowExpandable: (record) => record.items.length > 1,
          }}
        />
      </div>
    );
  }

  // mapping lại dữ liệu tổng theo nhà cung cấp
  mappingData(dataResponse) {
    if (dataResponse.length < 1) return [];
    var newData = [];
    var tempData = {};
    tempData.items = [];
    tempData.key = dataResponse[0].providerCode;
    tempData.providerCode = dataResponse[0].providerCode;
    tempData.providerName = dataResponse[0].providerName;
    tempData.khaiBao = dataResponse[0].khaiBao;
    tempData.canThucTe = dataResponse[0].canThucTe;
    tempData.chenhLech = dataResponse[0].chenhLech;
    tempData.tiLe = dataResponse[0].tiLe;
    tempData.qtyTotal = dataResponse[0].qtyTotal;
    tempData.trongLuongDaNhap = dataResponse[0].trongLuongDaNhap;
    tempData.trongLuongConLai = dataResponse[0].trongLuongConLai;
    tempData.tyLeHoanThanh = dataResponse[0].tyLeHoanThanh;
    tempData.items.push(dataResponse[0]);
    tempData.poNumber = dataResponse[0].poNumber;
    delete tempData.items[0].providerName;

    for (var i = 1; i < dataResponse.length; i++) {
      if (dataResponse[i].providerCode == dataResponse[i - 1].providerCode) {
        tempData.key = dataResponse[i].providerCode;
        // tempData.providerCode = dataResponse[i].providerCode
        // tempData.providerName = dataResponse[i].providerName
        tempData.khaiBao += dataResponse[i].khaiBao;
        tempData.canThucTe += dataResponse[i].canThucTe;
        tempData.chenhLech += dataResponse[i].chenhLech;
        tempData.tiLe += dataResponse[i].tiLe;
        tempData.qtyTotal += dataResponse[i].qtyTotal;
        tempData.trongLuongDaNhap += dataResponse[i].trongLuongDaNhap;
        tempData.trongLuongConLai += dataResponse[i].trongLuongConLai;
        tempData.tyLeHoanThanh += dataResponse[i].tyLeHoanThanh;
        delete dataResponse[i].providerName;
        tempData.items.push(dataResponse[i]);
      } else {
        if (tempData.items.length > 1) {
          delete tempData.poNumber;
        }

        tempData.tyLeHoanThanh = tempData.trongLuongDaNhap / tempData.qtyTotal;
        newData.push(tempData);
        tempData = {};
        tempData.items = [];
        tempData.key = dataResponse[i].providerCode;
        tempData.poNumber = dataResponse[i].poNumber;
        // tempData.providerCode = dataResponse[i].providerCode
        tempData.providerName = dataResponse[i].providerName;
        tempData.khaiBao = dataResponse[i].khaiBao;
        tempData.canThucTe = dataResponse[i].canThucTe;
        tempData.chenhLech = dataResponse[i].chenhLech;
        tempData.tiLe = dataResponse[i].tiLe;
        tempData.qtyTotal = dataResponse[i].qtyTotal;
        tempData.trongLuongDaNhap = dataResponse[i].trongLuongDaNhap;
        tempData.trongLuongConLai = dataResponse[i].trongLuongConLai;
        tempData.tyLeHoanThanh = dataResponse[i].tyLeHoanThanh;
        delete dataResponse[i].providerName;
        tempData.items.push(dataResponse[i]);
      }
    }
    return newData;
  }

  async getDataReport(pageSize, pageNumber, fromDate, toDate, plant) {
    this.setState({ loading: true });
    if (fromDate === undefined || toDate === undefined) {
      fromDate = moment().format("YYYY-MM-DD");
      toDate = moment().format("YYYY-MM-DD");
      // console.log(moment().format('YYYY-MM-DD'))
    }
    await GetReportGiaoNhan(pageSize, pageNumber, fromDate, toDate, plant).then(
      (objRespone) => {
        if (objRespone.isSuccess === true) {
          if (plant !== objRespone.item.plant[0].plantCode) {
            return;
          }
          this.setState({
            dataSource: this.mappingData(objRespone.item.data),
          });
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
