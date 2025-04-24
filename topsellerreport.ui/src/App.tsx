import React, { useEffect, useState } from 'react';
import axios from 'axios';

const API_BASE = 'http://localhost:5176/api/report'; // adjust if your backend is using another port

type TopSeller = {
  month: string;
  seller: string;
  totalOrders: number;
  totalSales: number;
};

function App() {
  const [branches, setBranches] = useState<string[]>([]);
  const [selectedBranch, setSelectedBranch] = useState('');
  const [topSellers, setTopSellers] = useState<TopSeller[]>([]);

  // Fetch branches on load
  useEffect(() => {
    axios.get(`${API_BASE}/branches`)
      .then(res => setBranches(res.data))
      .catch(() => alert('Failed to load branches'));
  }, []);

  // Fetch top sellers when branch changes
  useEffect(() => {
    if (!selectedBranch) return;

    axios.get(`${API_BASE}/top-sellers?branch=${encodeURIComponent(selectedBranch)}`)
      .then(res => setTopSellers(res.data))
      .catch(() => alert('Failed to load top sellers'));
  }, [selectedBranch]);

  return (
    <div style={{ padding: '2rem', fontFamily: 'Arial' }}>
      <h1>Top Sellers Report</h1>

      <label>
        Select Branch:{' '}
        <select
          value={selectedBranch}
          onChange={(e) => setSelectedBranch(e.target.value)}
        >
          <option value="">-- Choose a Branch --</option>
          {branches.map(branch => (
            <option key={branch} value={branch}>{branch}</option>
          ))}
        </select>
      </label>

      {topSellers.length > 0 && (
        <table style={{ marginTop: '2rem', width: '60%', borderCollapse: 'collapse' }}>
          <thead>
            <tr>
              <th style={{ border: '1px solid #ccc', textAlign: 'left', padding: '8px' }}>Month</th>
              <th style={{ border: '1px solid #ccc', textAlign: 'left', padding: '8px' }}>Seller</th>
              <th style={{ border: '1px solid #ccc', textAlign: 'left', padding: '8px' }}>Total Orders</th>
              <th style={{ border: '1px solid #ccc', textAlign: 'left', padding: '8px' }}>Total Price</th>
            </tr>
          </thead>
          <tbody>
            {topSellers.map((seller, idx) => (
              <tr key={idx}>
                <td style={{ width: '100px', border: '1px solid #ccc', padding: '8px' }}>{seller.month}</td>
                <td style={{ width: '100px', border: '1px solid #ccc', padding: '8px' }}>{seller.seller}</td>
                <td style={{ width: '100px', border: '1px solid #ccc', padding: '8px' }}>{seller.totalOrders}</td>
                <td style={{ width: '100px', border: '1px solid #ccc', padding: '8px' }}>${seller.totalSales.toFixed(2)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default App;
