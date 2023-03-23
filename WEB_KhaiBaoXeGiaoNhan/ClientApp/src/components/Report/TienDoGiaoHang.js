import React, { Component } from 'react';
import { Table, Button, DatePicker, Form, message, Typography, Tag, Row, Col } from 'antd';
import { myCheckAuth } from '../../Services/authServices';
import moment from 'moment';
import 'moment/locale/vi';
import locale from 'antd/es/date-picker/locale/vi_VN'
import { GetReportTienDoGiaoHang } from '../../Services/ReportService';
const layoutSearch = {
    labelCol: {
        span: 9,
    },
    wrapperCol: {
        span: 15,
    },
};



export class TienDoGiaoHang extends Component {
    static displayName = TienDoGiaoHang.name;

    // Check login
    componentWillMount() {
        myCheckAuth();
    }
    componentDidMount() {

    }
    constructor(props) {
        super(props);
        this.state = {
            dataSource: [],
            loading: true,
        };
    }




    componentWillMount() {
        this.getDataReport('', '', moment().format('YYYY-MM-DD'), moment().format('YYYY-MM-DD'))
    }

    // Search dữ liệu
    handleSearch(formValue) {
        console.log(formValue);
        this.getDataReport('', '', formValue.fromDate.format('YYYY-MM-DD'), formValue.toDate.format('YYYY-MM-DD'))

    }

    render() {
        const columns = [

            {
                title: 'Nhà cung cấp',
                dataIndex: 'providerName',
                key: 'providerName',
                sorter: (a, b) => a.providerName.length - b.providerName.length,
                // render: (text, row) => text.slice(0, 10),
                // render: (text, row) => <>{row.key === 'total' ? <Title level={5}>{text}</Title> : row.thoiGianToiDuKien.format('DD/MM/YYYY')}</>,
                // width: '6%',
            },
            {
                title: 'Mã đơn hàng',
                dataIndex: 'poNumber',
                key: 'poNumber',
                sorter: (a, b) => a.poNumber - b.poNumber,
                // render: (text, row) => text.slice(0, 10),
                // render: (text, row) => <>{row.key === 'total' ? <Title level={5}>{text}</Title> : row.thoiGianToiDuKien.format('DD/MM/YYYY')}</>,
                // width: '6%',
            },
            {
                title: 'Trọng lượng đơn hàng',
                dataIndex: 'qtyTotal',
                key: 'qtyTotal',
                align: 'right',
                // render: (text, row) => text.slice(0, 10),
                render: (text) => <>{new Intl.NumberFormat('de-DE').format(text) == 0 ? <>&#8210;</> : new Intl.NumberFormat('de-DE').format(text)}</>,
                // width: '6%',
            },
            {
                title: 'Đã giao',
                dataIndex: 'daGiao',
                key: 'daGiao',
                align: 'right',
                // render: (text, row) => text.slice(0, 10),
                render: (text) => <>{new Intl.NumberFormat('de-DE').format(text) == 0 ? <>&#8210;</> : new Intl.NumberFormat('de-DE').format(text)}</>,
                // width: '6%',
            },
            {
                title: 'Còn lại',
                dataIndex: 'conLai',
                key: 'conLai',
                align: 'right',
                // render: (text, row) => text.slice(0, 10),
                render: (text, row) => <>{new Intl.NumberFormat('de-DE').format(row.qtyTotal - row.daGiao) == 0 ? <>&#8210;</> : new Intl.NumberFormat('de-DE').format(row.qtyTotal - row.daGiao)}</>,
                // width: '6%',
            },
            {
                title: 'Tỷ lệ hoàn thành',
                dataIndex: 'tiLe',
                key: 'tiLe',
                align: 'right',
                // render: (text, row) => text.slice(0, 10),
                render: (text) => <>{new Intl.NumberFormat().format(text) == 0 ? <>&#8210;</> : new Intl.NumberFormat('de-DE', {
                    style: 'percent', minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                }).format(text)}</>,
                // width: '6%',
            },
            {
                title: 'Trạng thái',
                dataIndex: 'isComplete',
                key: 'isComplete',
                align: 'center',
                // render: (text, row) => text.slice(0, 10),
                render: (text, row) => <>{text == 1 ? row.tiLe > 0.995 ? <Tag color="success">Hoàn tất</Tag> : <Tag color="default">Kết thúc</Tag> : <Tag color="error">Chưa xong</Tag>}</>,
                // width: '6%',
            },

        ];




        return (
            <div>
                <h2 id="tabelLabel" >Trạng thái đơn hàng</h2>
                <p>Báo cáo trạng thái đơn hàng theo nhà cung cấp</p>
                <br />
                <b>Thời gian lên đơn hàng:</b>
                {/* <h4>Chi tiết giao nhận</h4> */}

                <div>
                    {/* <div align="baseline" style={{ width: '100%' }}>
            <Space>

              <Form
                onFinish={(value) => { this.searchListVehicleRegister(value) }}
                layout="inline">
                <Form.Item
                  name="time"
                  rules={[
                    { required: true, message: 'Chưa nhập thônng tin' },
                  ]}
                >
                  <RangePicker locale={locale} placeholder={['Từ ngày', 'Đến ngày']} format={'DD/MM/YYYY'} />
                </Form.Item>
                <Form.Item name="orderNumber"
                // rules={[
                //   { required: true, message: 'Chưa nhập thônng tin' },
                // ]}
                >
                  <Input placeholder="Mã đơn hàng"></Input>
                </Form.Item>
                <Form.Item>
                  <Button type="primary" htmlType="submit" >Tìm kiếm</Button>
                </Form.Item>

              </Form>
            </Space>
              <Text className='text-trong-luong' >Trọng lượng, kg</Text>

          </div>

          <Divider /> */}

                    <Form
                        // form={form}
                        name="advanced_search"
                        className="ant-advanced-search-form"
                        {...layoutSearch}
                        initialValues={{ fromDate: moment(), toDate: moment() }}
                        onFinish={(value) => this.handleSearch(value)}
                    // onFinish={onFinish}
                    >
                        {/* <Row gutter={24}>{getFields()}</Row> */}
                        <Row gutter={24}>
                            <Col span={6} >
                                <Form.Item
                                    labelAlign='left'
                                    name='fromDate'
                                    label='Từ ngày'
                                    rules={[{ required: true, message: 'Chưa chọn ngày' }]}
                                >
                                    <DatePicker style={{ width: '100%' }} locale={locale} placeholder='' format={'DD/MM/YYYY'} />
                                </Form.Item>
                            </Col>
                            <Col span={6} >
                                <Form.Item
                                    labelAlign='left'
                                    name='toDate'
                                    label='Đến ngày'
                                    rules={[{ required: true, message: 'Chưa chọn ngày' }]}
                                >
                                    <DatePicker style={{ width: '100%' }} locale={locale} placeholder='' format={'DD/MM/YYYY'} />
                                </Form.Item>
                            </Col>
                            <Col span={6} style={{
                                // textAlign: 'right',
                            }}>
                                <Button type="primary" htmlType="submit"> Tìm kiếm </Button>
                            </Col>
                        </Row>
                        {/* <Row gutter={24}>
                            <Col span={24} >
                                <Button type="primary" htmlType="submit"> Tìm kiếm </Button>
                                <Button
                                    style={{
                                        margin: '0 8px',
                                    }}
                                    onClick={() => {
                                        // form.resetFields();
                                    }}
                                > Xoá </Button>
                                <a
                                    style={{
                                        fontSize: 12,
                                    }}
                                    onClick={() => {
                                        // setExpand(!expand);
                                    }}
                                >
                                </a>
                            </Col>

                        </Row> */}

                    </Form>
                </div>
                <Table columns={columns}
                    // dataSource={this.state.listVehicleRegister}
                    className='my-table-detail-register'
                    loading={this.state.loading}
                    bordered
                    pagination={{ showSizeChanger: true }}
                    locale={{ emptyText: 'Không có thông tin' }}
                    dataSource={this.state.dataSource}
                />
            </div>

        );
    }
    async getDataReport(pageSize, pageNumber, fromDate, toDate) {
        this.setState({ loading: true })
        await GetReportTienDoGiaoHang(pageSize, pageNumber, fromDate, toDate, false).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                this.setState({
                    dataSource: objRespone.item.tienDoGiaoHang,
                })
            } else {
                message.error(objRespone.err.msgString)
            }
            this.setState({ loading: false })

        })
    }
}


