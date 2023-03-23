import { Form, Input, InputNumber, Button, Select, Radio } from 'antd';
import React, { Component } from 'react';
import jsonp from 'fetch-jsonp';
import querystring from 'querystring';
import { myCheckAuth } from '../../Services/authServices';


const layout = {
    labelCol: {
        span: 4,
    },
    wrapperCol: {
        span: 20,
    },
};
/* eslint-disable no-template-curly-in-string */

const validateMessages = {
    required: '${label} is required!',
    types: {
        email: '${label} is not a valid email!',
        number: '${label} is not a valid number!',
    },
    number: {
        range: '${label} must be between ${min} and ${max}',
    },
};


const { Option } = Select;

let timeout;
let currentValue;

function fetch(value, callback) {
    if (timeout) {
        clearTimeout(timeout);
        timeout = null;
    }
    currentValue = value;

    function fake() {
        const str = querystring.encode({
            code: 'utf-8',
            q: value,
        });
        jsonp(`https://suggest.taobao.com/sug?${str}`)
            .then(response => response.json())
            .then(d => {
                if (currentValue === value) {
                    const { result } = d;
                    const data = [];
                    result.forEach(r => {
                        data.push({
                            value: r[0],
                            text: r[0],
                        });
                    });
                    callback(data);
                }
            });
    }

    timeout = setTimeout(fake, 300);
}

export class VehicleRegister extends Component {

    // Check login
    componentWillMount() {
        myCheckAuth();
    }

    state = {
        data: [],
        value: undefined,
        vehicleType: 1,
    };

    handleSearch = value => {
        if (value) {
            fetch(value, data => this.setState({ data }));
        } else {
            this.setState({ data: [] });
        }
    };

    handleChange = value => {
        this.setState({ value });
    };
    onChange = e => {
        console.log('radio checked', e.target.value);
        this.setState({ vehicleType: e.target.value });
    };

    weightParser(val) {
        try {
            // for when the input gets clears
            if (typeof val === "string" && !val.length) {
                val = "0";
            }

            // detecting and parsing between comma and dot
            var group = new Intl.NumberFormat("en-us").format(1111).replace(/1/g, "");
            var decimal = new Intl.NumberFormat("en-us").format(1.1).replace(/1/g, "");
            var reversedVal = val.replace(new RegExp("\\" + group, "g"), "");
            reversedVal = reversedVal.replace(new RegExp("\\" + decimal, "g"), ".");
            //  => 1232.21 €

            // removing everything except the digits and dot
            reversedVal = reversedVal.replace(/[^0-9,]/g, "");
            //  => 1232,21

            // appending digits properly
            const digitsAfterDecimalCount = (reversedVal.split(",")[1] || []).length;
            const needsDigitsAppended = digitsAfterDecimalCount > 2;

            if (needsDigitsAppended) {
                reversedVal = reversedVal * Math.pow(10, digitsAfterDecimalCount - 2);
            }

            return Number.isNaN(reversedVal) ? 0 : reversedVal;
        } catch (error) {
            console.error(error);
        }
    };

    render() {

        const onFinish = (values) => {
            console.log(values);
        };
        const options = this.state.data.map(d => <Option key={d.value}>{d.text}</Option>);

        // const [value, setValue] = React.useState(1);


        return (
            <>
                <h2 >Đăng ký thông tin xe mới</h2>
                <Form {...layout} name="vehicle-register" onFinish={onFinish} validateMessages={validateMessages}>
                    <Form.Item
                        name={['vehicle', 'vehicle-number']}
                        label="Số xe"
                        rules={[
                            {
                                required: true,
                            },
                        ]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        name={['vehicle', 'owner']}
                        label="Đơn vị vận chuyển"
                        rules={[
                            {
                                required: true,
                            },
                        ]}>
                        <Select
                            showSearch
                            value={this.state.value}
                            placeholder={this.props.placeholder}
                            style={this.props.style}
                            defaultActiveFirstOption={false}
                            showArrow={false}
                            filterOption={false}
                            onSearch={this.handleSearch}
                            onChange={this.handleChange}
                            notFoundContent={null}
                        >
                            {options}
                        </Select>

                    </Form.Item>
                    <Form.Item
                        name={['vehicle', 'type']}
                        label="Kiểu xe"
                        rules={[
                            {
                                required: true,
                            },
                        ]}
                    >
                        <Radio.Group onChange={this.onChange} value={1}>
                            <Radio value={1}>Xe thường</Radio>
                            <Radio value={2}>Rơ mooc</Radio>
                            <Radio value={3}>Đầu kéo</Radio>
                        </Radio.Group>
                    </Form.Item>
                    <Form.Item
                        name={['vehicle', 'vehicle-weight']}
                        label="Trọng lượng bì"
                        rules={[
                            {
                                type: 'number',
                                min: 1,
                                max: 99999,
                                required: true,
                            },
                        ]}
                    >
                        {/* <InputNumber style={{
                            width: 200,
                        }}
                            formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                        /> */}
                        <InputNumber className="my-input-number" style={{
                            width: 200,
                        }}
                            formatter={value => new Intl.NumberFormat('de-DE').format(value)}
                            parser={this.weightParser}
                        />
                    </Form.Item>
                    <Form.Item name={['vehicle', 'trong-luong-dang-kiem']}
                        label="Trọng lượng bì"
                        rules={[
                            {
                                type: 'number',
                                min: 1,
                                max: 99999,
                                required: true,
                            },
                        ]}
                        label="Trọng lượng đăng kiểm">
                        <InputNumber style={{
                            width: 200,
                        }}
                            ormatter={value => new Intl.NumberFormat('de-DE').format(value)}
                            parser={this.weightParser}
                        />
                    </Form.Item>
                    <Form.Item name={['vehicle', 'ty-le-vuot']} label="Tỷ lệ vượt" initialValue={10}>
                        <InputNumber
                            defaultValue={10}
                            min={0}
                            max={100}
                            formatter={value => `${value}%`}
                            parser={value => value.replace('%', '')}
                        // onChange={onChange}
                        />
                    </Form.Item>
                    <Form.Item wrapperCol={{ ...layout.wrapperCol, offset: 4 }}>
                        <Button type="primary" htmlType="submit">
                            Lưu
                </Button>
                    </Form.Item>
                </Form>
            </>

        );
    }
}