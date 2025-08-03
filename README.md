# DataAnalyzeApi

ASP.NET Core Web API application for multi-parameter data analysis through similarity calculation and clustering algorithms. Processes datasets with mixed numerical and categorical parameters using custom-implemented algorithms.

## Technologies & Tools

### Core Technologies

- **Framework**: ASP.NET Core Web API (.NET 9.0)
- **Authentication**: ASP.NET Core Identity with JWT tokens
- **Database**: PostgreSQL with Entity Framework Core
- **Cache**: Redis for distributed caching
- **Containerization**: Docker & Docker Compose

### Libraries & Packages

- **AutoMapper**: Object-to-object mapping
- **Entity Framework Core**: ORM and database migrations
- **JWT Authentication**: Token-based security
- **Custom Implementation**: All clustering algorithms and similarity calculations

### Development & Testing

- **Unit Testing**: xUnit, Moq, AutoFixture, AutoMoq
- **Integration Testing**: TestContainers with PostgreSQL and Redis
- **CI/CD**: GitHub Actions with automated testing and deployment

## Features

### Data Management

- JSON-based dataset creation via REST API
- Support for numerical and categorical parameters
- Automatic parameter type detection during import
- Data validation ensuring type consistency across parameters
- Parameter weighting (0.1-10) and activation/deactivation

### Similarity Analysis

Full pairwise comparison algorithm calculating similarity scores (0-1) between all objects in a dataset. Supports:

- Mixed data types (numerical and categorical)
- Comma-separated values in categorical parameters
- Configurable parameter weights and activation
- Optional parameter inclusion in results

### Clustering Algorithms

Three custom-implemented clustering methods with:

- MinMax normalization for numerical data
- One-Hot encoding for categorical data
- Multiple distance metrics (Euclidean, Manhattan, Cosine for numerical; Hamming, Jaccard for categorical)
- PCA dimensionality reduction for 2D visualization coordinates

1. **K-Means**: Configurable clusters (2-100), iterations (10-1000)
2. **DBSCAN**: Epsilon (0.01-1.0), minimum points (1-20), noise detection
3. **Hierarchical Agglomerative**: Merge threshold (0.01-1.0), bottom-up approach

### Result Management

- Hash-based caching using MD5 of request parameters
- Multi-layer storage: Redis cache + PostgreSQL persistence
- Result retrieval by dataset, algorithm type, or globally
- Automatic cache invalidation and cleanup

## API Endpoints

### Authentication

**Base Route**: `/api/auth`

| Method | Endpoint    | Description                          | Auth Required |
| ------ | ----------- | ------------------------------------ | ------------- |
| POST   | `/login`    | User authentication with credentials | No            |
| POST   | `/register` | User registration                    | No            |
| POST   | `/logout`   | User logout                          | Yes           |

### Dataset Management

**Base Route**: `/api/datasets` (requires authentication)

| Method | Endpoint | Description                  | Permission |
| ------ | -------- | ---------------------------- | ---------- |
| GET    | `/`      | Retrieve all datasets        | User/Admin |
| GET    | `/{id}`  | Get dataset by ID            | User/Admin |
| POST   | `/`      | Create new dataset from JSON | User/Admin |
| DELETE | `/{id}`  | Delete dataset               | Admin only |

### Analysis Endpoints

**Base Routes**: `/api/analysis/similarity`, `/api/analysis/clustering` (requires authentication)

#### Similarity Analysis

```
POST /api/analysis/similarity/{datasetId}
```

**Request Body** (optional):

```json
{
  "parameterSettings": [
    {
      "parameterId": 1,
      "weight": 1.5,
      "isActive": true
    }
  ],
  "includeParameters": false
}
```

**Response**: List of similarity pairs with scores (0-1 range)

#### Clustering Analysis

```
POST /api/analysis/clustering/kmeans/{datasetId}
POST /api/analysis/clustering/dbscan/{datasetId}
POST /api/analysis/clustering/agglomerative/{datasetId}
```

**K-Means Request**:

```json
{
  "numberOfClusters": 5,
  "maxIterations": 200,
  "numericMetric": "Euclidean",
  "categoricalMetric": "Hamming",
  "parameterSettings": [...]
}
```

**DBSCAN Request**:

```json
{
  "epsilon": 0.2,
  "minPoints": 2,
  "numericMetric": "Euclidean",
  "categoricalMetric": "Hamming",
  "parameterSettings": [...]
}
```

**Agglomerative Request**:

```json
{
  "threshold": 0.2,
  "numericMetric": "Euclidean",
  "categoricalMetric": "Hamming",
  "parameterSettings": [...]
}
```

**Clustering Response**: Clusters with object assignments and 2D coordinates for visualization

### Result Retrieval

**Base Routes**: `/api/results/similarity`, `/api/results/clustering` (requires authentication)

| Method | Endpoint                                     | Description                                        |
| ------ | -------------------------------------------- | -------------------------------------------------- |
| GET    | `/`                                          | All analysis results of specified type             |
| GET    | `/dataset/{datasetId}`                       | Results by dataset                                 |
| GET    | `/dataset/{datasetId}/algorithm/{algorithm}` | Results by dataset and algorithm (clustering only) |

## Algorithm Details

### Similarity Calculation Process

1. **Data Preprocessing**: Filter active parameters and calculate numerical ranges
2. **Pair Generation**: Create all unique object pairs (no self-comparison)
3. **Parameter Comparison**:
   - **Numerical**: `similarity = 1 - |value1 - value2| / parameter_range`
   - **Categorical**: Jaccard coefficient on comma-separated values as sets
4. **Weighted Aggregation**: Sum weighted similarities divided by total weights
5. **Result**: Final similarity score (0-1) per object pair

### Clustering Workflow

1. **Data Normalization**: MinMax for numerical, One-Hot for categorical parameters
2. **Distance Calculation**: Apply selected metrics to normalized data
3. **Cluster Formation**: Execute chosen algorithm with specified parameters
4. **Dimensionality Reduction**: Apply PCA for 2D coordinate generation
5. **Result Assembly**: Package clusters with object coordinates

### Distance Metrics

**Numerical Metrics**:

- **Euclidean**: `sqrt(sum((a - b)²)) / max_possible_distance`
- **Manhattan**: `sum(|a - b|) / vector_length`
- **Cosine**: `1 - (dot_product / (|A| * |B|))`

**Categorical Metrics**:

- **Hamming**: `different_elements / total_elements`
- **Jaccard**: `1 - (intersection_size / union_size)`

## Data Model

### Authentication

- **ApplicationUser**: Extends IdentityUser with FirstName, LastName, RegisteredDate
- **Roles**: Default user role and admin role with policy-based authorization
- **JWT**: Token-based authentication with configurable expiration

## Example Data Source

The application processes datasets similar to the **Fragile States Index** (https://fragilestatesindex.org/excel/). Example dataset available in `DataAnalyzeApi.Integration/Data/` directory in JSON format, demonstrating the supported structure for multi-parameter country data.

## Deployment

### CI/CD Pipeline (GitHub Actions)

**Testing Stage**:

- Runs on Ubuntu with PostgreSQL and Redis services
- Executes unit and integration tests
- Validates build in Release configuration

**Deployment Stage** (master branch only):

- Builds Docker images on self-hosted runner
- Stops existing containers
- Deploys updated stack
- Cleans unused Docker images

### Production Environment

**Docker Stack** (7 containers):

```yaml
services:
  data-analyze-api: # Main application (port 8080)
  data-analyze-frontend: # Web interface
  postgresql: # Database with persistent volumes
  redis: # Distributed cache
  pgadmin: # Database management interface
  nginx-proxy: # Reverse proxy and API gateway
  portainer: # Container management
```

### Server Deployment

```bash
# Ensure Docker images are available on server
# Then start the complete stack
docker-compose up -d

# API available at: http://your-server/api/
# Swagger docs at: http://your-server/swagger/
```

**Nginx Configuration**:

- Routes `/api/*` to backend application
- Routes `/swagger*` to API documentation
- Routes all other traffic to frontend
- Handles CORS headers for cross-domain requests

## Testing

### Unit Tests (`DataAnalyzeApi.Unit`)

- Comprehensive coverage of services, mappers, and business logic
- Technologies: xUnit, Moq, AutoFixture, AutoMoq
- Focus on mathematical algorithms and data processing logic

### Integration Tests (`DataAnalyzeApi.Integration`)

- End-to-end API workflow testing with TestContainers
- Isolated PostgreSQL and Redis instances per test
- Real dataset processing with Fragile States Index data
- Complete authentication and authorization flow validation
