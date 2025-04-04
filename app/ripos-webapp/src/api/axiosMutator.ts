import api from './axiosConfig';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const customMutator = (args: any) => {
  const { url, method, params, data, ...rest } = args;
  return api({ url, method, params, data, ...rest });
};

export default customMutator;
