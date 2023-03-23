import { message, Modal, Form, Input, Table, Popconfirm, Space, Button, Divider, notification } from 'antd';
import React, { Component } from 'react';
import { myCheckAuth } from '../../Services/authServices';
import { AddDriver, GetDriverList, RemoveDriver, SearchDriver, UpdateDriver } from '../../Services/DriverService/driverService';
import { ExclamationCircleOutlined, WarningOutlined } from '@ant-design/icons';
import { GetListPOGiaKhac, ApprovePOGiaKhac, HuyPOGiaKhac } from '../../Services/KhaiPOGiaKhac';
import { SearchPOByNo } from '../../Services/OrderServices/orderService';

const { Column } = Table;
const { confirm } = Modal;

// const layoutAddDriver = {
//     labelCol: {
//         span: 6,
//     },
//     wrapperCol: {
//         span: 18,
//     },
// };


export class KhaiBaoPOGiaKhac extends Component {
    static displayName = KhaiBaoPOGiaKhac.name;

    constructor(props) {
        super(props);
        this.state = {
            dataPOSearch: [],
            dataPOList: [],
            loadingTblGiaKhac: true,
            searching: false,
        };
    }
    // Check login
    componentWillMount() {
        myCheckAuth();
    }

    componentDidMount() {
        this.getListPoGiaKhac()
    }



    handleCloseModal() {

    }

    // addDriver(record) {
    //     AddDriver(record).then((objRespone) => {
    //         if (objRespone.isSuccess === true) {
    //             message.success('Thêm tài xế thành công!')
    //             this.getDriverList();
    //             this.handleCloseModal();
    //         } else {
    //             message.error(objRespone.err.msgString)
    //         }
    //     })
    // }


    // Cập nhật thông tin tài xế
    // updateDriver(value) {
    //     value.driverId = this.state.seletedDriver.driverId
    //     UpdateDriver(value).then((objRespone) => {
    //         if (objRespone.isSuccess === true) {
    //             message.success('Cập nhật thành công!')
    //             this.getDriverList();
    //             this.handleCloseModal();
    //         } else {
    //             message.error(objRespone.err.msgString)
    //         }
    //     })

    // }

    handleConfirmPOGiaKhac(record) {
        // Call api lưu po được khai giá khác
        this.approvePOGiaKhac(record.poNumber)
    }


    // }

    handleDeleteItem(record) {
        // Call api xoá po được báo là giá khác
        console.log('Xoá Record')
        this.deleteKhaiGiaKhac(record.poNumber)
    }

    // hadlle tìm kím thông tin PO
    searchPOInfo(value) {
        console.log(value);
        this.getPOInfoList(value.poNumber)

    }

    // Show xác nhận xoá tài xế
    showConfirm(record) {
        confirm({
            title: 'Bạn có muốn xoá tài xế ' + record.driverName,
            icon: <WarningOutlined />,
            content: '',
            okText: 'Huỷ',
            cancelText: 'Xoá',
            // okType: 'default',
            keyboard: false,
            onOk() {

            },
            onCancel() {
                // Gọi APi xác nhận xoá tài xế
                RemoveDriver(record.driverId).then((objRespone) => {
                    if (objRespone.isSuccess === true) {
                        message.success("Xoá tài xế thành công");
                        this.getDriverList();
                    } else {
                        message.error(objRespone.err.msgString)
                    }
                })
            },
        });

    }

    render() {

        const columnsPoSearch = [
            {
                key: 'poNumber',
                title: 'Mã đơn hàng',
                dataIndex: 'poNumber',
                // width: '20%'
            },
            {
                title: 'Nhà cung cấp',
                dataIndex: 'providerName',
            },
            {
                title: 'Trọng lượng đơn hàng',
                align: 'right',
                dataIndex: 'qtyTotal',
                render: (text) => <>{new Intl.NumberFormat('de-DE').format(text)}</>,
            },

            {
                title: 'Vật tư',
                dataIndex: 'vatTu',
                render: (arr) => (arr.map(item => <p style={{ display: 'inline' }} key={item}>{item}<br /></p>)),
                // width: '20%',
            },

            {
                title: 'Giá khác',
                dataIndex: 'action',
                key: "action",
                align: 'right',
                render: (text, record) => (
                    <Popconfirm title="Xác nhận đơn hàng giá khác？"
                        icon={<WarningOutlined />}
                        okText='Có'
                        okType='default'
                        cancelText="Không"
                        onConfirm={() => this.handleConfirmPOGiaKhac(record)}
                    >
                        <Button size="small" >Xác nhận</Button>
                    </Popconfirm>

                ),
                // width: '10%',
            },

        ];


        const columnsPOList = [{
            title: 'Mã đơn hàng',
            dataIndex: 'poNumber',
            // width: '20%'
        },
        {
            title: 'Nhà cung cấp',
            dataIndex: 'providerName',
        },
        {
            title: 'Trọng lượng đơn hàng',
            align: 'right',
            dataIndex: 'qtyTotal',
            render: (text) => <>{new Intl.NumberFormat('de-DE').format(text)}</>,
            // width: '20%',
        },

        {
            title: 'Vật tư',
            dataIndex: 'vatTu',
            render: (arr) => (arr.map(item => <p style={{ display: 'inline' }} key={item}>{item}<br /></p>)),
            // width: '20%',
        },

        {
            title: 'Giá khác',
            dataIndex: 'action',
            key: "action",
            align: 'right',
            render: (text, record) => (

                <Popconfirm title="Bạn có muốn xoá？"
                    icon={<WarningOutlined />}
                    okText='Có'
                    okType='default'
                    cancelText="Không"
                    onConfirm={() => this.handleDeleteItem(record)}
                >
                    <Button type="primary" danger>Xoá</Button>
                </Popconfirm>
            ),
            // width: '10%',
        },]
        return (
            <div>
                <h2>Đơn hàng giá khác</h2>
                <p>Khai báo đơn hàng có giá khác với giá công bố</p>
                <br /><br />
                {/* <button className="btn btn-primary" onClick={this.showModalAddDriver}>Thêm tài xế</button> */}
                <Space align="baseline">
                    <Form onFinish={(value) => this.searchPOInfo(value)} layout="inline">
                        <Form.Item name="poNumber" label="Mã đơn hàng"
                            rules={[{
                                required: true,
                                message: 'Chưa nhập mã PO'
                            },
                            {
                                min: 4,
                                message: 'Nhập 4 ký tự trở lên'
                            }]}>
                            <Input
                            />
                        </Form.Item>
                        <Form.Item>
                            <Button type="primary" htmlType="submit">Tìm kiếm</Button>
                        </Form.Item>

                    </Form>
                </Space>
                <Divider />
                <Table dataSource={this.state.dataPOSearch}
                    loading={this.state.searching}
                    // locale={{ emptyText: 'Không có thông tin' }}
                    columns={columnsPoSearch}>
                </Table>
                <Divider />
                <h5>
                    Danh sách PO đã được báo giá khác với giá công bố
                </h5>
                <Table loading={this.state.loadingTblGiaKhac} columns={columnsPOList} dataSource={this.state.dataPOList}>

                </Table>
            </div>
        );


    }
    async getListPoGiaKhac() {
        this.setState({
            loadingTblGiaKhac: true
        })
        await GetListPOGiaKhac().then((objRespone) => {
            if (objRespone.isSuccess === true) {
                // mapping data
                var data = []
                data = objRespone.data.map((item) => ({
                    poNumber: item.pomasters.ponumber,
                    providerName: item.pomasters.providerName,
                    qtyTotal: item.pomasters.qtyTotal,
                    vatTu: item.polines.map((poLine) => poLine.productName)
                }))
                this.setState({
                    dataPOList: data
                })

            } else {
                this.setState({
                    dataPOList: []
                })
            }
            this.setState({
                loadingTblGiaKhac: false
            })
        })
    }

    // search thogn tinPO
    async getPOInfoList(poNumber) {
        this.setState({
            searching: true
        })
        await SearchPOByNo(poNumber).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                var data = []
                data = objRespone.data.map(item => ({
                    poNumber: item.pomasters.ponumber,
                    providerName: item.pomasters.providerName,
                    qtyTotal: item.pomasters.qtyTotal,
                    vatTu: item.polines.map(poLine => poLine.productName),
                }))
                this.setState({
                    dataPOSearch: data
                })

            } else {
                this.setState({
                    dataPOSearch: []
                })
            }
            this.setState({
                searching: false
            })
        })
    }

    // Xoá khai giá khác
    async deleteKhaiGiaKhac(poNumber) {
        await HuyPOGiaKhac(poNumber).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                this.getListPoGiaKhac()
                notification['success']({
                    message: 'Xoá thành công',
                    description:
                        `Đã bỏ đánh dấu đơn hàng ${poNumber} là đơn hàng giá khác`,
                });

            } else {
                this.setState({
                    dataPOList: []
                })
            }
        })
    }
    // search thogn tinPO
    async approvePOGiaKhac(poNumber) {
        await ApprovePOGiaKhac(poNumber).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                this.getListPoGiaKhac()
            } else {
                notification['success']({
                    message: 'Cập nhật thành công',
                    description:
                        `Đơn hàng ${poNumber} đã được đánh dấu là có giá khác với giá công bố`,
                });
            }
        })
    }
}
