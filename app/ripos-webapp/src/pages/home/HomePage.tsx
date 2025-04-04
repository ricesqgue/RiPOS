import { useGetApiBrands } from '@api/generated/brand/brand';
import { useAuthStore } from '@stores/authStore';
import { Button } from 'antd';

const HomePage = () => {
  const { data: brands, refetch } = useGetApiBrands();
  const { logout } = useAuthStore();
  const handleClick = () => {
    refetch();
    console.log('Brands:', brands);
  };

  const handleLogout = () => {
    logout();
  };

  return (
    <div>
      <Button onClick={handleClick}>CLick HERE</Button>
      <Button onClick={handleLogout}>Logout</Button>
    </div>
  );
};

export default HomePage;
