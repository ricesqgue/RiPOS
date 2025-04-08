import { Link } from 'react-router';

const HomePage = () => {
  return (
    <div>
      HOME PAGE <Link to={'/marcas'}>LINK</Link>
    </div>
  );
};

export default HomePage;
