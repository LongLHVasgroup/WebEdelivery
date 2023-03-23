import { Button, Col, DatePicker, Form, Row, Select, Typography } from "antd";
import locale from "antd/es/date-picker/locale/vi_VN";
import React from "react";
import classes from "./FormSearchFromTo.module.css";
const { Text } = Typography;
const { Option } = Select;
// const layoutSearch = {
//   labelCol: {
//     span: 9,
//   },
//   wrapperCol: {
//     span: 15,
//   },
// };
const FormSearchReportNhapHang = (props) => {
  const dateFormat = "DD/MM/YYYY";
  const options = props.plants.map((p) => (
    <Option key={p.companyCode} value={p.companyCode}>
      {p.companyName}
    </Option>
  ));
  return (
    <Form
      name="search_from"
      className={classes["form-search"]}
      // {...layoutSearch}
      initialValues={{ fromDate: props.from, toDate: props.to, plant : props.defaultPlant}}
      onFinish={props.onFinish}
    >
      <Row gutter={24}>
        <Col span={5} className={classes.column}>
          <Form.Item
            className={classes["form-item"]}
            labelAlign="left"
            name="fromDate"
            label="Từ ngày"
            rules={[{ required: true, message: "Chưa chọn ngày" }]}
          >
            <DatePicker
              className={classes.datePicker}
              locale={locale}
              placeholder=""
              format={dateFormat}
            />
          </Form.Item>
        </Col>
        <Col span={5} className={classes.column}>
          <Form.Item
            className={classes["form-item"]}
            labelAlign="left"
            name="toDate"
            label="Đến ngày"
            rules={[{ required: true, message: "Chưa chọn ngày" }]}
          >
            <DatePicker
              className={classes.datePicker}
              locale={locale}
              placeholder=""
              format={dateFormat}
            />
          </Form.Item>
        </Col>

        <Col span={8} className={classes.column}>
          <Form.Item
            className={classes["form-item"]}
            labelAlign="left"
            name="plant"
            label="Đơn vị"
            rules={[{ required: true, message: "Chưa chọn đơn vị" }]}
          >
            <Select allowClear>{options}</Select>
          </Form.Item>
        </Col>

        <Col span={3}>
          <Button type="primary" htmlType="submit">
            {" "}
            Tìm kiếm{" "}
          </Button>
        </Col>
        <Col
          span={3}
          className={classes.column}
          style={{
            alignSelf: "center",
            paddingRight: "20px",
          }}
        >
          <Text className={classes["text-trong-luong"]}>Trọng lượng, kg</Text>
        </Col>
      </Row>
    </Form>
  );
};

export default FormSearchReportNhapHang;
