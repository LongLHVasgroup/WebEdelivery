import {
    Button, Col, DatePicker,
    Form,
    message, Row, Select, Table, Typography
} from "antd";
import locale from "antd/es/date-picker/locale/vi_VN";
import moment from "moment";
import "moment/locale/vi";
import React, { Component } from "react";
import { LATEST_PLANT } from "../../Constant";
import { GetListCompany } from "../../Services/CompanyService";
import { GetReportTinhHinhDangKy } from "../../Services/ReportService";
import HeaderPage from "../Common/HeaderPage";
import { LatestPlant } from "../Common/LatestSelectedPlant";
import TextNumber from "../Common/TextNumber";
import TextPercent from "../Common/TextPercent";
const { Text } = Typography;
const { Option } = Select;
const { RangePicker } = DatePicker;

// const layoutSearch = {
//   labelCol: {
//     span: 9,
//   },
//   wrapperCol: {
//     span: 15,
//   },
// };

// Báo cáo tình hình vận chuyển-> liệt kê dữ liệu theo từng ngày

export class TinhHinhDangKy extends Component {
    static displayName = TinhHinhDangKy.name;

    // Check login
    // componentWillMount() {
    //     myCheckAuth();
    // }
    constructor(props) {
        super(props);
        this.state = {
            dataSource: [],
            loading: true,
            plants: [],
            selectedPlant: LatestPlant(),
            loading: true,
        };
    }


    componentDidMount() {
        this.getDonVi();
        this.getDataReport("", "", moment().format("YYYY-MM-DD"), moment().format("YYYY-MM-DD"), this.state.selectedPlant);
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
            formValue.time[0].format("YYYY-MM-DD"),
            formValue.time[1].format("YYYY-MM-DD"),
            formValue.plant
        );
    }

    render() {
        const columns = [
            {
                title: "Nhà cung cấp",
                dataIndex: "providerName",
                key: "providerName",
            },
            {
                title: "Số PO",
                dataIndex: "poNumber",
                key: "poNumber",
            },
            {
                title: "Số lượng đơn hàng",
                dataIndex: "qtyTotal",
                align: "right",
                render: (text) => <TextNumber value={text} />,
                key: "qtyTotal",
            },
            {
                title: "Số lượng đã giao",
                dataIndex: "daGiao",
                key: "daGiao",
                align: "right",
                render: (text) => <TextNumber value={text} />,
                // width: '6%',
            },
            {
                title: "Số xe giao hàng",
                dataIndex: "vehicleNumber",
                key: "vehicleNumber",
                align: "right",
                // width: '6%',
            },

            {
                title: "Giờ giao",
                dataIndex: "thoiGianToiThucTe",
                key: "thoiGianToiThucTe",
                align: "right",
                render: (text, row) => text ? <>{moment(text).format("HH:mm")}</> : <></>,
                // width: '6%',
            },

            {
                title: "Ngày giao hàng",
                dataIndex: "thoiGianToiThucTe",
                key: "thoiGianToiThucTe",
                align: "right",
                render: (text, row) => text ? <>{moment(text).format("DD/MM/YYYY")}</> : <>{moment(row.thoiGianToiDuKien).format("DD/MM/YYYY")}</>,
                // width: '6%',
            },

            {
                title: "Tải trọng xe",
                dataIndex: "trongLuongGiaoThucTe",
                key: "trongLuongGiaoThucTe",
                align: "right",
                render: (text, row ) => text ? <TextNumber value={text} /> : <TextNumber value={row.tlDuKien} />,
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
                    title="Tình hình đăng ký giao hàng"
                    description="Báo cáo tổng hợp xe đăng ký giao hàng"
                />

                <Form
                    // form={form}
                    name="advanced_search"
                    className="ant-advanced-search-form"
                    //   {...layoutSearch}
                    initialValues={{
                        time: [moment(), moment()],
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
                                name="time"
                                label="Ngày"
                                rules={[{ required: true, message: "Chưa chọn ngày" }]}
                            >
                                {/* <RangePicker
                  style={{ width: "100%" }}
                  locale={locale}
                  placeholder=""
                  format={"DD/MM/YYYY"}
                /> */}
                                <RangePicker
                                    //   onChange={(value) => {
                                    //     this.handleChangeGlobalTime("pick", value);
                                    //   }}
                                    locale={locale}
                                    format="DD/MM/YYYY"
                                    bordered={false}
                                    placeholder={["Từ ngày", "Đến ngày"]}
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
                            <Button type="primary" htmlType="submit" loading={this.state.loading}>
                                {" "}
                                Tìm kiếm{" "}
                            </Button>
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
    async getDataReport(pageSize, pageNumber, fromDate, toDate, plant) {
        this.setState({ loading: true });
        console.log(plant);
        await GetReportTinhHinhDangKy(fromDate, toDate, plant, false).then(
            (objRespone) => {
                if (objRespone.isSuccess === true) {
                    this.setState({
                        dataSource: objRespone.item,
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
