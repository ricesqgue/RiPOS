import { Outlet } from 'react-router';

const AppLayout = () => {
  return (
    <div>
      <h1>App Layout</h1>
      <h2>
        <Outlet />
      </h2>
    </div>
  );
};

export default AppLayout;
