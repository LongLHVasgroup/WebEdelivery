import React from 'react'
import { Modal, Form, Input, Button } from 'antd';



function ChangePasswordModal(props) {

    const layout = {
        labelCol: { span: 9 },
        wrapperCol: { span: 15 },
    };
    return (
        <Modal title='Thay đổi mật khẩu'
            centered
            visible={props.visible}
            onCancel={props.onCancel}
            footer={[
                <Button type="primary" form="formPass" key="submit" htmlType="submit">
                    Đổi mật khẩu
                </Button>
            ]}>
            <Form {...layout}
                id='formPass'
                title="Thay đổi mật khẩu"
                onFinish={props.onFinish}>
                <Form.Item
                    labelAlign='left'
                    name='currentPass'
                    label='Mật khẩu hiện tại'
                    rules={[
                        {
                            required: true,
                            message: 'Mật khẩu trống!'
                        },
                    ]}>
                    <Input.Password />
                </Form.Item>
                <Form.Item name='newPass'
                    label='Nhập mật khẩu mới'
                    labelAlign='left'
                    rules={[
                        {
                            required: true,
                            message: 'Mật khẩu mới trống!'
                        },
                    ]}>
                    <Input.Password />

                </Form.Item>
                <Form.Item name='repeatNewPass'
                    label='Nhập lại mật khẩu mới'
                    labelAlign='left'
                    rules={[
                        {
                            required: true,
                            message: 'Mật khẩu mới trống!'
                        },
                    ]}>
                    <Input.Password />
                </Form.Item>
            </Form>
        </Modal>
    )
}

export default ChangePasswordModal;