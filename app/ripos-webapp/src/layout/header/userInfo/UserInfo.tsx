import { useAuthStore } from '@stores/authStore';
import { Avatar, Flex } from 'antd';
import styles from './userInfo.module.scss';

interface UserInfoProps {
  collapsed: boolean;
}

const UserInfo = (props: UserInfoProps) => {
  const { userInfo } = useAuthStore();

  if (!userInfo) {
    return <></>;
  }

  const initials = `${userInfo.name![0]}${userInfo.surname![0]}`;
  const fullName = `${userInfo.name} ${userInfo.surname} ${userInfo.secondSurname}`.trimEnd();

  return (
    <Flex justify="center" align="center" gap={8}>
      <Avatar className={styles.avatar}>
        <span title={fullName}>{initials}</span>
      </Avatar>
      {!props.collapsed && <div>{fullName}</div>}
    </Flex>
  );
};

export default UserInfo;
