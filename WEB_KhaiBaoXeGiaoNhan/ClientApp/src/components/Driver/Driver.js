import { message, Modal, Form, Input, Table, DatePicker, Space, Button, Divider } from 'antd';
import React, { Component } from 'react';
import { myCheckAuth } from '../../Services/authServices';
import { AddDriver, GetDriverList, RemoveDriver, SearchDriver, UpdateDriver } from '../../Services/DriverService/driverService';
import { ExclamationCircleOutlined, WarningOutlined } from '@ant-design/icons';

const { Column } = Table;
const { confirm } = Modal;

const layoutAddDriver = {
    labelCol: {
        span: 6,
    },
    wrapperCol: {
        span: 18,
    },
};


export class Driver extends Component {
    static displayName = Driver.name;

    constructor(props) {
        super(props);
        this.state = {
            currentCount: 0,
            driverList: [],
            seletedDriver: {},
            visibleCreateDriver: false,
            visibleUpdateDriver: false,
        };
        this.showModalAddDriver = this.showModalAddDriver.bind(this);
        this.showModalUpdateDriver = this.showModalUpdateDriver.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
    }
    // Check login
    componentWillMount() {
        myCheckAuth();
    }

    componentDidMount() {
        this.getDriverList();
    }

    // Lấy danh sách tài xế
    getDriverList() {
        // Lấy danh sách tài xế
        GetDriverList().then((objRespone) => {
            if (objRespone.isSuccess === true) {
                this.setState({
                    loading: false,
                    driverList: objRespone.data
                })

            } else {
                message.error(objRespone.err.msgString)
            }
        })
    }

    handleCloseModal() {
        this.setState({
            visibleCreateDriver: false,
            visibleUpdateDriver: false
        })
    }

    addDriver(record) {
        AddDriver(record).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                message.success('Thêm tài xế thành công!')
                this.getDriverList();
                this.handleCloseModal();
            } else {
                message.error(objRespone.err.msgString)
            }
        })
    }


    // Cập nhật thông tin tài xế
    updateDriver(value) {
        value.driverId = this.state.seletedDriver.driverId
        UpdateDriver(value).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                message.success('Cập nhật thành công!')
                this.getDriverList();
                this.handleCloseModal();
            } else {
                message.error(objRespone.err.msgString)
            }
        })

    }


    showModalAddDriver() {
        this.setState({
            visibleCreateDriver: true
        })
    }

    showModalUpdateDriver() {
        this.setState({
            visibleCreateDriver: true
        })
    }

    // Hiện Modal update thong tin tài xế
    showUpdateDriver(record) {
        this.setState({
            visibleUpdateDriver: true,
            seletedDriver: record
        })


    }


    searchDriver(text) {
        // Goị API tìm kiếm tài xế theo tên hoặc cmnd
        message.loading("Đang tìm kiếm")
        console.log(text);
        SearchDriver(text.search).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                this.setState({
                    loading: false,
                    driverList: objRespone.data
                })

            } else {
                message.error(objRespone.err.msgString)
            }
        })

    }

    // Show xác nhận xoá tài xế
    showConfirm(record) {
        var that = this
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
                        that.getDriverList();
                    } else {
                        message.error(objRespone.err.msgString)
                    }
                })
            },
        });

    }

    render() {
        return (
            <div>
                <h2>Danh sách tài xế</h2>
                <p>Quản lý danh sách tài xế</p>
                <br /><br />
                {/* <button className="btn btn-primary" onClick={this.showModalAddDriver}>Thêm tài xế</button> */}
                <Space align="baseline">
                    <Form onFinish={(value) => this.searchDriver(value)} layout="inline">
                        <Form.Item name="search"
                            rules={[{ required: true, message: 'Chưa nhập thônng tin' }]}>
                            <Input
                                placeholder="Tên tài xế/CMND/GPLX"
                            />
                        </Form.Item>
                        <Form.Item>
                            <Button type="primary" htmlType="submit">Tìm kiếm</Button>
                        </Form.Item>

                    </Form>
                    <Button type="primary" onClick={this.showModalAddDriver}>Thêm tài xế</Button>
                </Space>
                <Divider />
                <Table dataSource={this.state.driverList}
                    locale={{ emptyText: 'Chưa có dữ liệu' }} >

                    <Column title="Họ tên" dataIndex="driverName" key="driverName" width='45%'/>
                    <Column title="Số CMND/GPLX" dataIndex="driverCardNo" key="driverCardNo" width='45%' />
                    {/* <Column title="Năm Sinh" dataIndex="dob" key="dob" /> */}

                    <Column
                        align='center'
                        title="Thay đổi"
                        key="action"
                        align='center'
                        render={(text, record) => (
                            <Space size="middle">
                                <a
                                    key="edit"
                                    onClick={() => {
                                        this.showUpdateDriver(record);
                                    }}
                                >
                                    <Button size="small" >Sửa</Button>
                                </a>
                                <a
                                    key="delete"
                                    onClick={() => {
                                        this.showConfirm(record)
                                    }}
                                >
                                    <Button size="small" danger>Xoá</Button>
                                </a>
                            </Space>
                        )}
                    />
                </Table>

                {/* Tạo mới tài xế */}
                <Modal

                    visible={this.state.visibleCreateDriver}
                    title="Thêm tài xế"
                    destroyOnClose={true}
                    onCancel={this.handleCloseModal}
                    footer={[
                        <Button type="primary" form="formAdd" key="submit" htmlType="submit">
                            Lưu
                        </Button>
                    ]}
                >
                    <Form
                        {...layoutAddDriver}
                        id="formAdd"
                        onFinish={(values) => { this.addDriver(values) }}>
                        <Form.Item
                            labelAlign='left'
                            name="driverName"
                            label="Tên tài xế"
                            normalize={(input) => input.toUpperCase()}
                            rules={[
                                {
                                    required: true,
                                    message: 'Chưa nhập tên tài xế!'
                                },
                            ]}>
                            <Input />
                        </Form.Item>
                        <Form.Item
                            labelAlign='left'
                            name="driverCardNo"
                            label="CMND/GPLX"
                            rules={[
                                {
                                    required: true,
                                    message: 'Chưa nhập thông tin!'
                                },
                            ]}>
                            <Input />
                        </Form.Item>
                        {/* <Form.Item
                            labelAlign='left'
                            name="dob"
                            label="Năm sinh"
                        >
                            <DatePicker picker="year" placeholder="Chọn năm sinh" style={{ width: '50%' }} />
                        </Form.Item> */}
                    </Form>
                </Modal>



                {/* Update thông tin tài xế */}
                <Modal

                    visible={this.state.visibleUpdateDriver}
                    title="Cập nhật thông tin tài xế"
                    destroyOnClose={true}
                    onCancel={this.handleCloseModal}
                    footer={[
                        <Button type="primary" form="formUpdate" key="submit" htmlType="submit">
                            Cập nhật
                        </Button>
                    ]}
                >
                    <Form
                        {...layoutAddDriver}
                        id="formUpdate"
                        initialValues={this.state.seletedDriver}
                        onFinish={(values) => { this.updateDriver(values) }}>
                        <Form.Item
                            labelAlign='left'
                            name="driverName"
                            label="Tên tài xế"
                            normalize={(input) => input.toUpperCase()}
                            rules={[
                                {
                                    required: true,
                                    message: 'Chưa nhập tên tài xế!'
                                },
                            ]}>
                            <Input />
                        </Form.Item>
                        <Form.Item
                            labelAlign='left'
                            name="driverCardNo"
                            label="CMND/GPLX"
                            rules={[
                                {
                                    required: true,
                                    message: 'Chưa nhập thông tin!'
                                },
                            ]}>
                            <Input />
                        </Form.Item>
                        {/* <Form.Item
                            labelAlign='left'
                            name="dob"
                            label="Năm sinh"
                        >
                            <DatePicker picker="year" placeholder="Chọn năm sinh" style={{ width: '50%' }} />
                        </Form.Item> */}
                    </Form>
                </Modal>
            </div>
        );
    }
}
