import { useState } from 'react';

type FilterManager<T, K> = {
  serverFilters: T;
  clientFilters: K;
  setServerFilters: (updates: Partial<T>) => void;
  setClientFilters: (updates: Partial<K>) => void;
  resetAllFilters: () => void;
};

const useTableFilters = <T extends object, K extends object>(
  initialServerFilters: T,
  initialClientFilters: K
): FilterManager<T, K> => {
  const [serverFilters, setServer] = useState<T>(initialServerFilters);
  const [clientFilters, setClient] = useState<K>(initialClientFilters);

  return {
    serverFilters,
    clientFilters,
    setServerFilters: (updates) => setServer((prev) => ({ ...prev, ...updates })),
    setClientFilters: (updates) => setClient((prev) => ({ ...prev, ...updates })),
    resetAllFilters: () => {
      setServer(initialServerFilters);
      setClient(initialClientFilters);
    },
  };
};

export default useTableFilters;
