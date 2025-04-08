import { Spin } from 'antd';
import { SpinSize } from 'antd/lib/spin';

interface LoadingProps {
  fullscreen?: boolean;
  size?: SpinSize;
}
const Loading = ({ fullscreen, size }: LoadingProps) => {
  return <Spin fullscreen={fullscreen} size={size}></Spin>;
};

export default Loading;
