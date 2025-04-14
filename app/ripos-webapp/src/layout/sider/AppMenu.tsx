import { IconProp } from '@fortawesome/fontawesome-svg-core';
import {
  faCashRegister,
  faHome,
  faList,
  faPalette,
  faRulerCombined,
  faStore,
  faTags,
  faUserGroup,
  faUsers,
  faUserTag,
  faVenusMars,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Menu, MenuProps } from 'antd';
import { useEffect, useMemo, useState } from 'react';
import { Link, matchPath, useLocation } from 'react-router';

type MenuItem = Required<MenuProps>['items'][number];

type AppMenuItem = {
  key: string;
  label: string;
  icon?: IconProp;
  navigateTo?: string;
  subItems?: AppMenuItem[];
  pathPattern?: string[];
};

interface AppMenuProps {
  collapsed: boolean;
}

const menuItems: AppMenuItem[] = [
  {
    key: '1',
    label: 'Inicio',
    icon: faHome,
    navigateTo: '/',
  },
  {
    key: '2',
    label: 'Marcas',
    icon: faTags,
    navigateTo: '/marcas',
  },
  {
    key: '3',
    label: 'Clientes',
    icon: faUserGroup,
  },
  {
    key: '4',
    label: 'Proveedores',
    icon: faUserTag,
  },
  {
    key: '5-1',
    label: 'Géneros',
    icon: faVenusMars,
    navigateTo: '/generos',
  },
  {
    key: '5-2',
    label: 'Colores',
    icon: faPalette,
  },
  {
    key: '5-3',
    label: 'Tallas',
    icon: faRulerCombined,
    navigateTo: '/tallas',
  },
  {
    key: '6',
    label: 'Cajas Registradoras',
    icon: faCashRegister,
    navigateTo: '/cajas-registradoras',
  },
  {
    key: '7',
    label: 'Categorías',
    icon: faList,
    navigateTo: '/categorias',
  },
  {
    key: '8',
    label: 'Sucursales',
    icon: faStore,
  },
  {
    key: '9',
    label: 'Usuarios',
    icon: faUsers,
  },
];

const buildMenuItems = (items: AppMenuItem[]): MenuItem[] =>
  items.map((item) => ({
    key: item.key,
    icon: item.icon ? <FontAwesomeIcon icon={item.icon} /> : undefined,
    label: item.navigateTo ? <Link to={item.navigateTo}>{item.label}</Link> : item.label,
    children: item.subItems ? buildMenuItems(item.subItems) : undefined,
  }));

const AppMenu = (props: AppMenuProps) => {
  const [selectedKeys, setSelectedKeys] = useState<string[]>([]);
  const items = useMemo(() => buildMenuItems(menuItems), []);
  const location = useLocation();

  const handleActiveItems = () => {
    const sKeys: string[] = [];

    const findMatchPath = (items: AppMenuItem[], parents: string[] = []) => {
      for (const item of items) {
        const { navigateTo, pathPattern } = item;
        const matchDirect =
          navigateTo && matchPath({ path: navigateTo, end: true }, location.pathname);
        const matchFromPatterns = pathPattern?.some((pattern) =>
          matchPath({ path: pattern, end: true }, location.pathname)
        );

        if (matchDirect || matchFromPatterns) {
          sKeys.push(item.key);
          return true;
        }

        if (item.subItems && findMatchPath(item.subItems, [...parents, item.key])) {
          return true;
        }
      }

      return false;
    };

    findMatchPath(menuItems);
    setSelectedKeys(sKeys);
  };

  useEffect(() => {
    handleActiveItems();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location.pathname]);

  return (
    <div>
      <Menu
        style={{ borderInlineEnd: 'none' }}
        mode="inline"
        inlineCollapsed={props.collapsed}
        items={items}
        selectedKeys={selectedKeys}
      />
    </div>
  );
};

export default AppMenu;
