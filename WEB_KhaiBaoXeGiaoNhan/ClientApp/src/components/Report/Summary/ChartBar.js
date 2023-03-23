import React from "react";
import { Spin } from "antd";
import { Bar } from "react-chartjs-2";
import { FormatedNumber } from "../../Common/TextNumber";



const ChartBar = (props) => {


    const options = {
        tooltips: {
            callbacks: {
                label: function (tooltipItem, data) {
                    return tooltipItem;
                },
            },
        },
        scales: {
            yAxes: {
                ticks: {
                    // display: false,
                    beginAtZero: true,
                    // format: FormatedNumber(value),
                    callback: function (value, index, values) {
                        return FormatedNumber(value / 1000);
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

            xAxes: {
                gridLines: {
                    display: false,
                },
                ticks: {
                    family: "Glypha VO",
                },
                title: {
                    display: true,
                    text: "Nhà cung cấp",
                    font: {
                        family: "Glypha VO",
                    },
                },
            },
        },
        animation: {
            duration: 0,
        },
        font: {
            size: 14,
        },
        plugins: {
            datalabels: {
                color: "#666",
                display: "auto", // nếu đủ khoản cách thì mới hiện số
                anchor: "end",
                align: "end",
                labels: {
                    title: {
                        font: {
                            family: "Glypha VO",
                        },
                    },
                },
                formatter: function (value, context) {
                    return FormatedNumber(value);
                },
            },
            legend: {
                labels: {
                    // This more specific font property overrides the global property
                    font: {
                        family: "Glypha VO",
                    },
                },
                titles: {
                    display: false,
                },
                display: false,
            },
            title: {
                font: {
                    family: "Glypha VO",
                },
                display: true,
                // text: 'Phân bổ trọng lượng đã nhập theo nhà cung cấp'
            },
            tooltip: {
                enabled: true,
                // mode: 'single',
                callbacks: {
                    label: function (context) {
                        var label = context.dataset.label || "";

                        if (label) {
                            label += ": ";
                        }
                        if (context.parsed.y !== null) {
                            label += FormatedNumber(context.parsed.y) + " kg";
                        }
                        return label;
                    },
                },
            },
        },
    };
    return (<Spin spinning={props.loading}>
        <Bar
            data={props.data}
            options={options}
        />
    </Spin>)
}
export default ChartBar;