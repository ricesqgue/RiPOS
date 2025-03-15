interface AppContextProviderProps {
  children: React.ReactNode;
}

const AppContextProvider = (props: AppContextProviderProps) => {
  return <>{props.children}</>;
};

export default AppContextProvider;
