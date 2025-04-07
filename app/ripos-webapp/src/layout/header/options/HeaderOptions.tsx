import {
  faArrowRightFromBracket,
  faEllipsisVertical,
  faMoon,
  faSun,
  faUser,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useAuthStore } from '@stores/authStore';
import { useThemeStore } from '@stores/themeStore';
import { Button, Dropdown, MenuProps } from 'antd';

const HeaderOptions = () => {
  const { darkMode, toggleTheme } = useThemeStore();
  const { logout } = useAuthStore();

  const items: MenuProps['items'] = [
    {
      key: '1',
      label: 'Perfil',
      onClick: () => {
        console.log('Profile clicked');
      },
      icon: <FontAwesomeIcon icon={faUser} />,
    },
    {
      key: '2',
      label: darkMode ? 'Modo claro' : 'Modo oscuro',
      onClick: () => {
        toggleTheme();
      },
      icon: <FontAwesomeIcon icon={darkMode ? faSun : faMoon} />,
    },
    {
      key: '3',
      label: 'Cerrar sesión',
      onClick: () => {
        logout();
      },
      icon: <FontAwesomeIcon icon={faArrowRightFromBracket} />,
    },
  ];

  return (
    <Dropdown menu={{ items }} trigger={['click']}>
      <Button
        type="text"
        icon={<FontAwesomeIcon style={{ color: '#FFF' }} icon={faEllipsisVertical} />}
        size="middle"
        shape="circle"
      ></Button>
    </Dropdown>
  );
};
export default HeaderOptions;
