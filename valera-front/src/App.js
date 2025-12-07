import { BrowserRouter, Routes, Route, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import './App.css';
import ValeraList, { api } from './Valeras/AllValera.jsx';
import CreateValeraForm from './Valeras/CreateValera.jsx';
import ValeraSearch from './Valeras/ValeraSearch.jsx';

const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [emailReg, SetRegEmail] = useState('');
  const [password, setPassword] = useState('');
  const [PasswordReg, SetRegPassword] = useState('');
  const [error, setError] = useState('');
  const [errorReg, SetRegError] = useState('');
  const [UserName, setUserName] = useState('');
  const navigate = useNavigate();

  const HandleLogin = async (e) => {
      e.preventDefault();
      setError('');

      try {
          const response = await api.post('/Auth/login', {
              email: email,
              password: password,
          });

          const token = response.data.token;

          if (token) {
              localStorage.setItem('jwtToken', token);
              navigate('/');
            } else {
              setError('Ошибка входа: Токен не получен.');
          }
      } catch (err) {
          const errorMessage = 'Неверный Email или Пароль.';
          setError(errorMessage);
      }
      finally {
        setEmail('');
        setPassword('');
      }
  };

  const HandleCreate = async (e) => {
    e.preventDefault(); 
    setError('');

    try {
      const resp = await api.post('/Auth/register', {
        email : emailReg,
        password: PasswordReg,
        username: UserName
      });
      const token = resp.data.token;
      if (token) {
        localStorage.setItem('jwtToken', token);
        navigate('/');

      } else {
        SetRegError('Ошибка регистрации со стороны сервера'); 
       }
    }
    catch (err) {
      const erMes = err.response?.data?.errors;
      SetRegError(erMes);
    }
    finally {
        setUserName('');
        SetRegEmail('');
        SetRegPassword('');
    }
  }

  return (
      <div style={{ padding: '20px' }}>
          <h2>Вход в систему</h2>
          <form onSubmit={HandleLogin}>
              <div>
                  <label>Email:</label>
                  <input 
                      type="email" 
                      value={email} 
                      onChange={(e) => setEmail(e.target.value)} 
                      required 
                  />
              </div>
              <div>
                  <label>Пароль:</label>
                  <input 
                      type="password" 
                      value={password} 
                      onChange={(e) => setPassword(e.target.value)} 
                      required 
                  />
              </div>
              {error && <p style={{ color: 'red' }}>{error}</p>}
              <button type="submit">Войти</button>
          </form>
          <div>
            Зарегистрироваться:
              <form onSubmit={HandleCreate}>
              <div>
                  <label>Email:</label>
                  <input 
                      type="email" 
                      value={emailReg} 
                      onChange={(e) => SetRegEmail(e.target.value)} 
                      required 
                  />
              </div>
              <div>
                  <label>Пароль:</label>
                  <input 
                      type="password" 
                      value={PasswordReg} 
                      onChange={(e) => SetRegPassword(e.target.value)} 
                      required 
                  />
              </div>
              <div>
                <label>Имя пользователя:</label>
                <input type="text" value={UserName} onChange= {(e) => {setUserName(e.target.value)}} required></input>
                {errorReg && <p style={{ color: 'red' }}>{errorReg}</p>}
              </div>
              <button type="submit">Зарегистрироваться</button>
              </form>
        </div>
      </div>
  );
}

const HomePage = () => {
  const token = localStorage.getItem('jwtToken');
  if (!token) {
      return LoginPage();
    }
  return (
  <div className="App">
  <header className="App-header">
    Ваши Валеры:
    <ValeraSearch />
    <CreateValeraForm onValeraCreated={() => window.dispatchEvent(new Event("valera:updated"))}/>
    <ValeraList />
  </header>
  </div>
  );
}

function App() {
  return (
    <BrowserRouter>
    <Routes>
      <Route path = "/" element={<HomePage />} />
      <Route path="/login" element={<LoginPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
