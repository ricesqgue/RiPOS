import { useAuthStore } from '@stores/authStore';
import { Avatar, Flex, Grid } from 'antd';
import styles from './userInfo.module.scss';
import { useEffect, useState } from 'react';

const { useBreakpoint } = Grid;

const UserInfo = () => {
  const { userInfo } = useAuthStore();
  const [collapsed, setCollapsed] = useState(false);
  const screens = useBreakpoint();

  useEffect(() => {
    setCollapsed(!screens.md);
  }, [screens.md]);

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
      {!collapsed && <div>{fullName}</div>}
    </Flex>
  );
};

export default UserInfo;
