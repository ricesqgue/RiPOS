import { Grid } from 'antd';

const { useBreakpoint } = Grid;

const BrandsPage = () => {
  const screens = useBreakpoint();
  return (
    <>
      <h1>Brands page</h1>
      <div>
        {screens.xs && <p>Extra small screen</p>}
        {screens.sm && <p>Small screen</p>}
        {screens.md && <p>Medium screen</p>}
        {screens.lg && <p>Large screen</p>}
        {screens.xl && <p>Extra large screen</p>}
        {screens.xxl && <p>2x Extra large screen</p>}
      </div>
    </>
  );
};

export default BrandsPage;
