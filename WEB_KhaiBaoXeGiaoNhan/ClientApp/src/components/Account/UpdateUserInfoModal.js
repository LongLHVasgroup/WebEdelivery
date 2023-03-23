import React from "react";
import { Modal, Form, Input, Button } from "antd";
function UpdateUserInfoModal(props) {

    const layoutUpdateInfo = {
        labelCol: { span: 6 },
        wrapperCol: { span: 18 },
    };
    return (
        <Modal title='Cập nhật thông tin cá nhân'
            visible={props.visible}
            centered
            footer={[
                <Button type="primary" form="formInfo" key="submit" htmlType="submit">
                    Cập nhật
                </Button>
            ]}
            onCancel={props.onCancel}>
            <Form {...layoutUpdateInfo}
                initialValues={props.initialValues}
                id='formInfo'
                title="Cập nhật thông itn tài khoản"
                onFinish={props.onFinish}>
                <Form.Item labelAlign='left' name='fullName' label='Họ và tên' >
                    <Input />
                </Form.Item>

                <Form.Item labelAlign='left' name='phone' label='Số điện thoại' >
                    <Input />
                </Form.Item>

                <Form.Item labelAlign='left' name='email' label='Email' >
                    <Input />
                </Form.Item>

            </Form>
        </Modal>
    )
}

export default UpdateUserInfoModal;