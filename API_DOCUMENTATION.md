# CrudPark Backend API - Documentación

## 🔗 URL Base
```
http://localhost:5229/api
```

## 🎯 Endpoints Principales

### 1. Dashboard (Pantalla Principal)
```javascript
// Obtener métricas en tiempo real
GET /api/Dashboard/metrics

Response:
{
  "success": true,
  "data": {
    "vehiclesInside": 15,
    "todayIncome": 350000,
    "activeSubscriptions": 45,
    "expiringSoon": 8,
    "expiredSubscriptions": 3,
    "todayEntries": 87,
    "todayExits": 72
  }
}
```

### 2. Clientes (CRUD)
```javascript
// Listar todos los clientes
GET /api/Customers

// Crear cliente
POST /api/Customers
Body:
{
  "fullName": "Juan Pérez",
  "email": "juan@email.com",
  "phone": "3001234567",
  "identificationNumber": "1234567890",
  "vehicleIds": [1, 2]  // Opcional
}

// Actualizar cliente
PUT /api/Customers/{id}
Body:
{
  "fullName": "Juan Pérez Actualizado",
  "email": "juannuevo@email.com"
}

// Eliminar cliente (soft delete)
DELETE /api/Customers/{id}
```

### 3. Vehículos (CRUD)
```javascript
// Crear vehículo
POST /api/Vehicles
Body:
{
  "licensePlate": "ABC123",
  "vehicleType": "Car",  // Car, Motorcycle, Truck
  "brand": "Toyota",
  "model": "Corolla",
  "color": "Blanco"
}

// Buscar por placa
GET /api/Vehicles/by-plate/ABC123
```

### 4. Mensualidades (CRUD)
```javascript
// Crear mensualidad
POST /api/Subscriptions
Body:
{
  "customerId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-02-01",
  "amountPaid": 150000,
  "maxVehicles": 2,
  "vehicleIds": [1, 2]
}

// Obtener próximas a vencer
GET /api/Subscriptions/expiring-soon?days=3

// Agregar vehículo a mensualidad
POST /api/Subscriptions/{id}/vehicles
Body:
{
  "vehicleId": 3
}
```

### 5. Operadores (CRUD)
```javascript
// Crear operador
POST /api/Operators
Body:
{
  "fullName": "Carlos López",
  "email": "carlos@crudpark.com",
  "username": "clopez",
  "password": "password123"
}
```

### 6. Tarifas (CRUD)
```javascript
// Obtener tarifa activa
GET /api/Rates/active

// Crear tarifa
POST /api/Rates
Body:
{
  "rateName": "Tarifa 2025",
  "hourlyRate": 3000,
  "fractionRate": 1000,
  "dailyCap": 30000,
  "gracePeriodMinutes": 30,
  "effectiveFrom": "2025-01-01"
}

// Calcular cobro
POST /api/Rates/calculate-fee
Body:
{
  "entryTime": "2025-01-15T08:00:00Z",
  "exitTime": "2025-01-15T11:30:00Z"
}
```

### 7. Reportes
```javascript
// Ingresos diarios/semanales/mensuales
GET /api/Reports/income?period=day

// Ocupación
GET /api/Reports/occupancy?days=7

// Comparativa mensualidades vs invitados
GET /api/Reports/subscribers-vs-guests?days=30
```

## 🎨 Estructura de Respuestas

Todas las respuestas siguen este formato:

**Éxito:**
```json
{
  "success": true,
  "message": "Operación exitosa",
  "data": { ... }
}
```

**Error:**
```json
{
  "success": false,
  "message": "Descripción del error",
  "error": "Detalles técnicos"
}
```

## 🚀 Ejemplos de Uso en Vue.js
```javascript
// services/api.js
import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5229/api',
  headers: {
    'Content-Type': 'application/json'
  }
});

export default api;

// services/dashboardService.js
import api from './api';

export const dashboardService = {
  getMetrics: () => api.get('/Dashboard/metrics'),
  getRecentActivity: (limit = 10) => api.get(`/Dashboard/recent-activity?limit=${limit}`)
};

// services/customerService.js
import api from './api';

export const customerService = {
  getAll: () => api.get('/Customers'),
  getById: (id) => api.get(`/Customers/${id}`),
  create: (data) => api.post('/Customers', data),
  update: (id, data) => api.put(`/Customers/${id}`, data),
  delete: (id) => api.delete(`/Customers/${id}`)
};

// Uso en componente Vue
import { dashboardService } from '@/services/dashboardService';

export default {
  data() {
    return {
      metrics: null
    }
  },
  async mounted() {
    try {
      const response = await dashboardService.getMetrics();
      this.metrics = response.data.data;
    } catch (error) {
      console.error('Error:', error);
    }
  }
}
```

## 📊 Gráficas (Chart.js)
```javascript
// Ejemplo para gráfica de ingresos
import { reportsService } from '@/services/reportsService';

async getIncomeData() {
  const response = await reportsService.getIncome('week');
  const data = response.data.data;
  
  this.chartData = {
    labels: data.map(d => new Date(d.date).toLocaleDateString()),
    datasets: [{
      label: 'Ingresos Diarios',
      data: data.map(d => d.totalIncome),
      backgroundColor: 'rgba(75, 192, 192, 0.2)',
      borderColor: 'rgba(75, 192, 192, 1)',
      borderWidth: 1
    }]
  };
}
```

## ⚠️ Notas Importantes

1. **CORS está configurado** para `http://localhost:5173` y `http://localhost:3000`
2. **No hay autenticación JWT** por ahora (las peticiones son públicas)
3. **Fechas:** Usar formato ISO 8601: `2025-01-15T08:00:00Z`
4. **Enums:** VehicleType: `Car`, `Motorcycle`, `Truck`
5. **Soft Delete:** Los DELETE no eliminan físicamente, solo marcan como inactivos