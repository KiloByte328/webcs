import React, { useState } from 'react';
import axios from 'axios';

function ValeraSearch({ onSelect }) {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSearch = async (e) => {
    const value = e.target.value;
    setQuery(value);
    setError('');

    if (value.trim() === '') {
      setResults([]);
      return;
    }

    setLoading(true);
    try {
      const response = await axios.get('https://localhost:63627/api/Valera', {
        params: { search: value }
      });

      setResults(response.data);
    } catch (err) {
      console.error(err);
      setError('Ошибка при поиске Валер');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ marginTop: '1rem' }}>
      <input
        type="text"
        value={query}
        onChange={handleSearch}
        placeholder="🔍 Поиск Валеры..."
        style={{
          padding: '0.5rem',
          width: '220px',
          borderRadius: '4px',
          border: '1px solid #ccc'
        }}
      />

      {loading && <p>Загрузка...</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      <div>
        {results.map(v => (
          <div
            key={v.id}
            onClick={() => onSelect?.(v)}
            style={{ cursor: 'pointer', marginTop: '5px' }}
          >
            🧍 {v.name} (HP: {v.hp}, MP: {v.mp})
          </div>
        ))}
      </div>
    </div>
  );
}

export default ValeraSearch;
