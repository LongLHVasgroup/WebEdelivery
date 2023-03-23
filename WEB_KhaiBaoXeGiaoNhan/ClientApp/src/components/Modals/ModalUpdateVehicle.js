import React, { Component } from 'react';
import { Modal, Button, Input, Form, Select, message } from 'antd';
import { SearchDriver } from '../../Services/DriverService/driverService';
import { SearchBSX, UpdateVehicle } from '../../Services/VehicleService/vehicleService';

const layout = {
    labelCol: {
        span: 6,
    },
    wrapperCol: {
        span: 18,
    },
};
const { Option } = Select;


class ModalVehicleComponent extends Component {
    constructor() {
        super();
        this.state = {
            taiXeList: [],
            romoocList: [],
        };
    }

    // Tìm kiếm Tên tài xế
    handleSearchTaiXe(text) {
        if (text.length >= 3) {
            SearchDriver(text).then((objRespone) => {
                if (objRespone.isSuccess === true) {
                    this.setState({
                        taiXeList: objRespone.data
                    })
                } else {
                    this.setState({
                        taiXeList: []
                    })
                }
            })
        } else {
            this.setState({
                taiXeList: []
            })
        }
    }

    handleSearchRomooc(text) {
        if (text.length >= 3) {
            SearchBSX(text, 'romooc').then((objRespone) => {
                if (objRespone.isSuccess === true) {
                  
                    this.setState({
                      romoocList: objRespone.data,
                    });
                } else {
                  this.setState({
                    romoocList: [],
                  });
                }
              });

        } else {
            this.setState({
                taiXeList: []
            })
        }
    }

    // Update vehicle
    handleUpdateVehicle(values) {
        if(values.romoocId ===null || values.romoocId === undefined){
            values.romoocId = "00000000-0000-0000-0000-000000000000"
        }
        if(values.driverId === null || values.driverId === undefined){
            values.driverId ='00000000-0000-0000-0000-000000000000'
        }
        UpdateVehicle(values.vehicleId, values.driverId, values.romoocId).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                message.success(objRespone.err.msgString);
                this.props.onSuccess()
            } else {
                
            }
        })
    }

    render() {
        const optionsTaiXe = this.state.taiXeList.map(d => <Option key={d.driverId}>{d.driverName}  -  {d.driverCardNo}</Option>);
        const optionsRomooc = this.state.romoocList.map((d) => (
            <Option key={d.vehicleId} value={d.vehicleId}>
              {d.vehicleNumber}
            </Option>
          ));
        return (
            <Modal title="Cập nhật thông tin"
                visible={this.props.visible}
                onCancel={this.props.onCancel}
                destroyOnClose={true}
                footer={[
                    <Button type="primary" form="formUpdate" key="submit" htmlType="submit">Cập nhật</Button>
                ]}>
                <Form {...layout} id='formUpdate' initialValues={this.props.initialValues}
                    onFinish={values => { this.handleUpdateVehicle(values) }}
                >
                    <Form.Item name='vehicleId' hidden>

                    </Form.Item>
                    <Form.Item labelAlign='left' label='Biển số xe' name='vehicleNumber' >
                        <Input disabled>
                        </Input>

                    </Form.Item>
                    <Form.Item labelAlign='left' name='romoocId' label='Romooc'>
                        <Select
                            showSearch
                            filterOption={false}
                            onSearch={value => { this.handleSearchRomooc(value) }}
                            notFoundContent=''
                            defaultOpen={()=>this.setState({ romoocList: this.props.listRomooc })}
                        >
                            {optionsRomooc}
                        </Select>
                    </Form.Item>
                    <Form.Item labelAlign='left' name='driverId' label='Tài xế'>
                        <Select
                            showSearch
                            filterOption={false}
                            onSearch={value => { this.handleSearchTaiXe(value) }}
                            notFoundContent=''
                            defaultOpen={()=>this.setState({ taiXeList: this.props.listTaiXe })}
                        >
                            {optionsTaiXe}
                        </Select>
                    </Form.Item>
                </Form>

            </Modal>
        );
    }

}

export default ModalVehicleComponent;

