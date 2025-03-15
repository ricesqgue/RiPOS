import React from 'react';
import ReactDOM from 'react-dom/client';
import '@styles/index.scss';
import '@fontsource/ubuntu/400.css';
import '@fontsource/ubuntu/500.css';

import App from './App';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
);
