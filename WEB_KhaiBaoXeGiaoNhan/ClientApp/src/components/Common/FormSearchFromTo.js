import React from 'react';
import { Form, Button, DatePicker, Typography, Row, Col } from 'antd';
import locale from 'antd/es/date-picker/locale/vi_VN';
import classes from './FormSearchFromTo.module.css';
const { Text } = Typography;
const layoutSearch = {
    labelCol: {
        span: 9,
    },
    wrapperCol: {
        span: 15,
    },
};
const FormSearchFromTo = (props) => {

    const dateFormat = 'DD/MM/YYYY'
    return <Form
        name="search_from"
        className={classes['form-search']}
        {...layoutSearch}
        initialValues={{ fromDate: props.from, toDate: props.to }}
        onFinish={props.onFinish}
    >
        <Row gutter={24} >
            <Col span={6} className={classes.column}>
                <Form.Item
                    className={classes['form-item']}
                    labelAlign='left'
                    name='fromDate'
                    label='Từ ngày'
                    rules={[{ required: true, message: 'Chưa chọn ngày' }]}

                >
                    <DatePicker className={classes.datePicker} locale={locale} placeholder='' format={dateFormat} />
                </Form.Item>
            </Col>
            <Col span={6} className={classes.column}>
                <Form.Item
                    className={classes['form-item']}
                    labelAlign='left'
                    name='toDate'
                    label='Đến ngày'
                    rules={[{ required: true, message: 'Chưa chọn ngày' }]}

                >
                    <DatePicker className={classes.datePicker} locale={locale} placeholder='' format={dateFormat} />
                </Form.Item>
            </Col>
            <Col span={6}  >
                <Button type="primary" htmlType="submit"> Tìm kiếm </Button>
            </Col>
            <Col span={6} className={classes.column} style={{
                alignSelf: 'center',
                paddingRight: '20px'

            }}>
                <Text className={classes['text-trong-luong']} >Trọng lượng, kg</Text>
            </Col>
        </Row>
    </Form>
}

export default FormSearchFromTo;