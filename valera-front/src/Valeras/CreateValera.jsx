import React, { useState } from 'react';
import { api } from './AllValera.jsx';

function CreateValeraForm({ onValeraCreated }) {
  const [visible, setVisible] = useState(false);
  const [form, setForm] = useState({
    name: '',
    hp: 100,
    mp: 0,
    ft: 0,
    cf: 0,
    mn: 0
  });
  const [message, setMessage] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: value === '' ? '' : Number(value) || value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage('');

    try {
      const response = await api.post(
        '/Valera',
        form,
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      );
      const data = response.data;
      setMessage(`✅ Валера создан! ID: ${data.id}`);

      onValeraCreated?.(data);

      setForm({
        name: '',
        hp: 100,
        mp: 0,
        ft: 0,
        cf: 0,
        mn: 0
      });
      setVisible(false);
    } catch (error) {
      console.error('Ошибка при создании Валеры:', error);
      if (error.response) {
        setMessage(
          `Ошибка ${error.response.status}: ${
            error.response.data?.title || 'Не удалось создать Валеру'
          }`
        );
      } else {
        setMessage('Ошибка соединения с сервером');
      }
    }
  };

  return (
    <div style={{ marginBottom: '1rem' }}>
      <button onClick={() => setVisible(!visible)}>
        {visible ? 'Закрыть форму' : 'Создать новую Валеру'}
      </button>

      {visible && (
        <form
          onSubmit={handleSubmit}
          style={{
            marginTop: '1rem',
            border: '1px solid #ccc',
            padding: '1rem',
            borderRadius: '5px'
          }}
        >
          <label>
            Name:
            <input
              type="text"
              name="name"
              value={form.name}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            HP:
            <input
              type="number"
              name="hp"
              min="0"
              max="100"
              value={form.hp}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            MP:
            <input
              type="number"
              name="mp"
              min="0"
              max="100"
              value={form.mp}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            FT:
            <input
              type="number"
              name="ft"
              min="0"
              max="100"
              value={form.ft}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            CF:
            <input
              type="number"
              name="cf"
              min="-10"
              max="10"
              value={form.cf}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            MN:
            <input
              type="number"
              name="mn"
              min="0"
              value={form.mn}
              onChange={handleChange}
            />
          </label>
          <br />
          <button type="submit">Создать Валеру</button>
        </form>
      )}

      {message && <p>{message}</p>}
    </div>
  );
}

export default CreateValeraForm;
