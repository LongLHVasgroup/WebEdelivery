import React from "react";
import { Descriptions } from "antd";
const CompanyInfo = (props) => {
  return (
    <React.Fragment>
      <Descriptions column={1} bordered title="Thông tin công ty">
        <Descriptions.Item label="Tên công ty">{props.cty}</Descriptions.Item>
        <Descriptions.Item label="Mã số thuế">
          {props.taxCode}
        </Descriptions.Item>
        <Descriptions.Item label="Địa chỉ ">{props.address}</Descriptions.Item>
      </Descriptions>
    </React.Fragment>
  );
};

export default CompanyInfo;
