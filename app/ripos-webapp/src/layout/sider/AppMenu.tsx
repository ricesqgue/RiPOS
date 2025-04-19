import { IconProp } from '@fortawesome/fontawesome-svg-core';
import {
  faBarcode,
  faCashRegister,
  faList,
  faPalette,
  faRulerCombined,
  faShirt,
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
    key: 'pos',
    label: 'Punto de venta',
    icon: faBarcode,
    navigateTo: '/',
  },
  {
    key: 'products',
    label: 'Productos',
    icon: faShirt,
    subItems: [
      {
        key: 'products-categories',
        label: 'Categorías',
        icon: faList,
        navigateTo: '/productos/categorias',
      },
      {
        key: 'products-colors',
        label: 'Colores',
        icon: faPalette,
        navigateTo: '/productos/colores',
      },
      {
        key: 'products-genders',
        label: 'Géneros',
        icon: faVenusMars,
        navigateTo: '/productos/generos',
      },
      {
        key: 'products-brands',
        label: 'Marcas',
        icon: faTags,
        navigateTo: '/productos/marcas',
      },
      {
        key: 'products-sizes',
        label: 'Tallas',
        icon: faRulerCombined,
        navigateTo: '/productos/tallas',
      },
    ],
  },

  {
    key: 'customers',
    label: 'Clientes',
    icon: faUserGroup,
    navigateTo: '/clientes',
  },
  {
    key: 'vendors',
    label: 'Proveedores',
    icon: faUserTag,
    navigateTo: '/proveedores',
  },

  {
    key: 'store',
    label: 'Tienda',
    icon: faStore,
    subItems: [
      {
        key: 'cashRegisters',
        label: 'Cajas',
        icon: faCashRegister,
        navigateTo: '/tienda/cajas-registradoras',
      },
      {
        key: 'users',
        label: 'Usuarios',
        icon: faUsers,
      },
    ],
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
