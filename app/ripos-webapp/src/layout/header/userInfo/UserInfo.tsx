import { useAuthStore } from '@stores/authStore';
import { Avatar, Flex } from 'antd';

const UserInfo = () => {
  const { userInfo } = useAuthStore();

  if (!userInfo) {
    return <></>;
  }

  const initials = `${userInfo.name![0]}${userInfo.surname![0]}`;
  const fullName = `${userInfo.name} ${userInfo.surname} ${userInfo.secondSurname}`.trimEnd();

  return (
    <Flex justify="center" align="center" gap={8}>
      <Avatar>{initials}</Avatar>
      <div>{fullName}</div>
    </Flex>
  );
};

export default UserInfo;
