import React from 'react';
import { render, screen, waitFor, fireEvent } from '@testing-library/react';
import App from './App';

// this MUST come after importing App
jest.mock('axios');
import axios from 'axios';
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('Top Sellers App', () => {
  it('renders dropdown with branch options', async () => {
    mockedAxios.get.mockResolvedValueOnce({ data: ['Branch 1', 'Branch 2'] });

    render(<App />);
    await waitFor(() => {
      const options = screen.getAllByRole('option');
    expect(options).toHaveLength(3); // "-- Choose a Branch --", "Branch A", "Branch B"
    expect(options[1]).toHaveTextContent('Branch 1');
    expect(options[2]).toHaveTextContent('Branch 2');
    });

    

  });

  it('fetches and displays top sellers when branch is selected', async () => {
    mockedAxios.get
      .mockResolvedValueOnce({ data: ['Branch 1'] })
      .mockResolvedValueOnce({
        data: [
          {
            month: 'January',
            seller: 'John',
            totalOrders: 12,
            totalSales: 4500
          }
        ]
      });

    render(<App />);
    await waitFor(() => screen.getByText('Branch 1'));

    fireEvent.change(screen.getByRole('combobox'), {
      target: { value: 'Branch 1' }
    });

    await waitFor(() => {
      expect(screen.getByText('January')).toBeInTheDocument();
      expect(screen.getByText('John')).toBeInTheDocument();
      expect(screen.getByText('$4500.00')).toBeInTheDocument();
    });
  });
});
