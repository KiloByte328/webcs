import logo from './logo.svg';
import './App.css';
import ValeraList from './Valeras/AllValera.jsx';
import CreateValeraForm from './Valeras/CreateValera.jsx';
import ValeraSearch from './Valeras/ValeraSearch.jsx';
import ValeraStats from './Valeras/ValeraStats.jsx';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        Ваш Валера:
      <ValeraSearch />
      <CreateValeraForm />
      <ValeraList />
      </header>
    </div>
  );
}

export default App;
