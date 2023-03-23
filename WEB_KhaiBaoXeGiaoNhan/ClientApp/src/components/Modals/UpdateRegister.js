import React, { useRef, useEffect } from "react";
import {
  Modal,
  Button,
  Input,
  Form,
  Select,
  InputNumber,
  DatePicker,
} from "antd";
import SelectTaiXe from "../Common/SelectTaiXe";

const layout = {
  labelCol: { span: 9 },
  wrapperCol: { span: 15 },
};

const UpdateRegister = (props) => {
  const onUpdateHandler = (value) => {
    console.log(value);
  };
  const formRef = useRef();

  // nhận value từ selectTaiXe
  const onChangeTaiXe = (taiXe) => {
    formRef.current.setFieldsValue({
      driverid: taiXe.key,
    });
    console.log(formRef.current);
  };
  // useEffect(() => {
  //   console.log(props.selectedRecord);
  // });
  return (
    <Modal
      title="Cập nhật thông tin"
      visible={props.visible}
      onCancel={props.onCancel}
      destroyOnClose={true}
      footer={[
        <Button
          type="primary"
          form="formUpdate"
          key="submit"
          htmlType="submit"
          loading={props.saving}
        >
          Cập nhật
        </Button>,
      ]}
    >
      <Form
        ref={formRef}
        id="formUpdate"
        initialValues={props.selectedRecord}
        {...layout}
        onFinish={onUpdateHandler}
      >
        <Form.Item
          labelAlign="left"
          name="thoiGianToiDuKien"
          label="Ngày vận chuyển"
        >
          <DatePicker format="DD/MM/YYYY" disabled style={{ width: "100%" }} />
        </Form.Item>
        <Form.Item
          labelAlign="left"
          name="soDonHang"
          label="Mã đơn hàng"
          rules={[
            {
              required: true,
              message: "Chưa chọn đơn hàng",
            },
          ]}
        >
          <Select
            style={props.style}
            // showArrow={false}
            // disabled={props.loadingPO}
            filterOption={false}
            notFoundContent={<>Không có đơn hàng</>}
            // onChange={(value, option) => this.handleChangeOrder(option)}
            disabled
          >
            {/* {optionsOrder} */}
          </Select>
        </Form.Item>

        <Form.Item labelAlign="left" name="vehicleNumber" label="Biển số xe">
          <Input disabled />
        </Form.Item>

        <Form.Item
          labelAlign="left"
          name="driverid"
          label="Tài xế"
          rules={[{ required: true, message: "Chưa chọn tài xế" }]}
        >
          {/* <Select
            showSearch
            style={props.style}
            showArrow={false}
            filterOption={false}
            onSearch={(value) => {
              this.handleSearchTaiXe(value);
            }}
            notFoundContent={
              <Button onClick={this.showCreateDriver}>Tạo mới</Button>
            }
          >
            {optionsTaiXe}
          </Select> */}
          <SelectTaiXe onChangeTaiXe={onChangeTaiXe} />
        </Form.Item>

        <Form.Item
          labelAlign="left"
          name="vatTuUpdate"
          label="Mặt hàng"
          rules={[{ required: true, message: "Chưa chọn mặt hàng" }]}
        >
          <Select
            mode="multiple"
            allowClear
            // placeholder="Please select"
            // defaultValue={}
            // onChange={handleChange}
          >
            {/* {optionsVatTu} */}
          </Select>
        </Form.Item>

        <Form.Item
          labelAlign="left"
          name="trongLuongGiaoDuKien"
          label="Trọng lượng hàng (kg)"
          rules={[
            {
              type: "number",
              min: 0,
              max: 99999,
              message: "Vui lòng nhập đúng giá trị",
            },
          ]}
        >
          <InputNumber
            className="my-input-number"
            style={{ width: "100%", textAlign: "right" }}
            formatter={(value) => new Intl.NumberFormat("de-DE").format(value)}
            // parser={this.weightParser}
          />
        </Form.Item>
        {/* <Form.Item labelAlign='left' name="assets" label='Tài sản theo xe'>
                <TextArea />
              </Form.Item> */}
      </Form>
    </Modal>
  );
};

export default UpdateRegister;
