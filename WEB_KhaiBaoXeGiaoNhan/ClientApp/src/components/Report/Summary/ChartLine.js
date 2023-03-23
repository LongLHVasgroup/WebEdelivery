import React from 'react'
import { Card, Spin } from 'antd';
import ChartTitle from './ChartTitle';
import { FormatedNumber } from '../../Common/TextNumber';
import { Line } from "react-chartjs-2";
import classes from './ChartLine.module.css'
const ChartLine = (props) => {
    const options = {
        scales: {
            yAxes: {
                // display: false,
                beginAtZero: true,
                ticks: {
                    callback: function (value, index, values) {
                        return FormatedNumber(value / 1000);
                        // return ''
                    },
                },
                display: true,
                title: {
                    display: true,
                    text: "x 1000",
                    font: {
                        family: "Glypha VO",
                        size: 11,
                    },
                },
            },

            xAxes: [
                {
                    gridLines: {
                        display: false,
                    },
                    ticks: {
                        fontFamily: "Glypha VO",
                    },
                },
            ],
        },
        animation: {
            duration: 0,
        },
        plugins: {
            legend: {
                display: false,
                labels: {
                    // This more specific font property overrides the global property
                    font: {
                        family: "Glypha VO",
                    },
                },
                title: {
                    display: false,
                },
            },
            tooltip: {
                enabled: true,
                // mode: 'single',
                callbacks: {
                    label: function (context) {
                        return FormatedNumber(context.parsed.y);
                    },
                },
            },
            datalabels: {
                color: "#666",
                display: "auto", // nếu đủ khoản cách thì mới hiện số
                anchor: "end",
                align: "end",
                offset: -1,
                labels: {
                    title: {
                        font: {
                            family: "Glypha VO",
                        },
                    },
                },
                formatter: function (value, context) {
                    if (value != 0) return FormatedNumber(value);
                    else return "";
                },
            },
        },
    };
    return (<Card
        title={
            <ChartTitle title='Biểu đồ trọng lượng sắt phế liệu đã nhập' />
        }
        extra={<p style={{ margin: "0px" }}>Đơn vị tính: kg</p>}
        className={classes['chart-line']}
    >
        <Spin spinning={props.loading}>
            <Line
                data={props.data}
                options={options}
                height={100}
            />
        </Spin>
    </Card>)
}
export default ChartLine;