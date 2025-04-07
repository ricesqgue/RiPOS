import { useAuthStore } from '@stores/authStore';
import { Flex } from 'antd';

const StoreInfo = () => {
  const { storeId, availableStores } = useAuthStore();
  const store = availableStores.find((store) => store.id === storeId);

  return (
    <Flex justify="center" align="center" gap={8}>
      <div>{store?.name}</div>
    </Flex>
  );
};

export default StoreInfo;
