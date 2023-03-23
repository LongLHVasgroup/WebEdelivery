import React from 'react'
import { Card, Spin, Statistic } from 'antd'
import TextPercent from '../../Common/TextPercent'
import ChartTitle from './ChartTitle'
const TongTrongLuong = (props) => {
    return (
        <Card
            title={
                <ChartTitle title='Sắt phế liệu đã nhập' />
            }
        >
            <p
                className="ant-statistic-title"
                style={{
                    marginTop: "-24px",
                    marginBottom: "0px",
                    textAlign: "end",
                }}
            >
                Đơn vị tính: kg
            </p>
            <Spin spinning={props.loading}>
                <Card bordered={false} className="none-padding-left-top">
                    <Statistic
                        title="Trong nước"
                        groupSeparator="."
                        decimalSeparator=","
                        value={
                            props.noiDia === null ? "-" : props.noiDia
                        }
                    />
                    <p className="ant-statistic-title">
                        <TextPercent
                            value={props.noiDia / props.total}
                        />
                    </p>
                </Card>
                <Card bordered={false} className="none-padding-left">
                    <Statistic
                        title="Nhập khẩu"
                        groupSeparator="."
                        decimalSeparator=","
                        value={
                            props.nhapKhau === null ? "-" : props.nhapKhau
                        }
                    />
                    <p className="ant-statistic-title">
                        <TextPercent
                            value={props.nhapKhau / props.total}
                        />
                    </p>
                </Card>
                <Card bordered={false} className="none-padding-left">
                    <Statistic
                        title="Tổng"
                        groupSeparator="."
                        decimalSeparator=","
                        value={props.total === null ? "-" : props.total}
                    />
                </Card>
            </Spin>
        </Card>
    )
}
export default TongTrongLuong;