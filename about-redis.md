# History of Caching

- **1940s--1960s**: Concept of fast-access memory emerged with early
  computer architectures.

- **1970s--1980s**: CPU caches introduced to bridge the speed gap
  between processors and main memory.

- **1990s**: Web caching gained traction with the growth of the internet
  (e.g., proxy caches, browser caches).

- **2000s**: In-memory distributed caches like **Memcached (2003)** and
  **Redis (2009)** emerged to handle scale in web applications.

- **2010s--present**: Caching becomes essential in cloud-native,
  microservices, and high-throughput architectures.


# Base Requirements of Caching

**1. Performance & Speed**

The primary purpose of a cache is to provide extremely fast data access,
significantly outperforming the primary data source. This is typically
achieved by using in-memory storage, targeting sub-millisecond or
microsecond response times.

**2. Eviction Management**

To manage finite cache capacity, automatic policies are required to
remove items. Key strategies include **LRU (Least Recently
Used)**, **LFU (Least Frequently Used)**, and **TTL-based
expiration** to make room for new data.

**3. Data Consistency & Freshness**

Mechanisms must exist to ensure cached data does not become stale and
reflects the source of truth. This involves **cache
invalidation** techniques (e.g., write-through,
cache-aside), **expiration controls (TTL)**, and support for eventual or
strong consistency models.

**4. Scalability**

The cache should be able to handle increased load, typically by
supporting **horizontal scaling** in distributed environments. This
allows the system to grow by adding more cache nodes.

**5. Reliability & Fault Tolerance**

The cache must be resilient to failures, especially in distributed
setups. Features like **data replication**, **persistence** to disk, and
high availability prevent data loss and maintain service during node
failures.

**6. Concurrency & Thread Safety**

The cache must operate correctly in multi-threaded environments,
ensuring data integrity and preventing race conditions during
simultaneous read/write operations.

**7. Security**

For sensitive data, caches should provide **encryption** (at rest and in
transit) and **access controls** to prevent unauthorized reading or
modification of cached items.

**8. Integration & Usability**

The cache should offer easy integration with application frameworks
(e.g., .NET Core) through simple APIs and provide **serialization
support** for storing and retrieving complex data types.


# Categories of Usage

| Category                             | Scope / Architecture                                  | Primary Use Case                                                | Key Characteristics                                                       | Common Examples                                                                   |
|--------------------------------------|-------------------------------------------------------|-----------------------------------------------------------------|---------------------------------------------------------------------------|-----------------------------------------------------------------------------------|
| **In‑Memory / Local Cache**          | Single application instance (process memory)          | Fast, short‑lived data in a single‑server app; session storage. | Fastest access (microseconds), lost on restart, limited to server RAM.    | IMemoryCache (.NET), Guava Cache (Java), Caffeine.                                |
| **Distributed Cache**                | Multiple servers/services across a network            | Shared state in scaled‑out apps (web farms, microservices).     | Shared data store, network latency, scalable, supports high availability. | **Redis**, Memcached, Hazelcast, NCache.                                          |
| **HTTP / CDN Cache**                 | Between client and server (edge/proxy)                | Static assets, public API responses, global content delivery.   | Geographic distribution, reduces origin load, leverages HTTP headers.     | **Cloudflare**, Akamai, Varnish, browser cache.                                   |
| **Application‑Level / Output Cache** | Application server layer                              | Caching entire rendered output (HTML, API responses).           | Bypasses repeated processing logic, often tied to request/response cycle. | [[ASP.NET Output Caching, Django cache framework.]{.underline}](https://asp.net/) |
| **Data / Database Cache**            | Data‑access layer (app or DB engine)                  | Query results, computed datasets, object graphs.                | Reduces database load, requires invalidation logic, can be transparent.   | Redis (for query results), PostgreSQL buffers, Hibernate L2 cache.                |
| **Persistent Cache**                 | Any cache that adds durability (local or distributed) | Critical cached data that is expensive to recompute.            | Survives restarts/failures via disk storage; adds slight I/O overhead.    | Redis with RDB/AOF, Ehcache with disk store.                                      |

# Strategic Use of Caching

**When to Apply**

| Scenario Category                  | Short Description                                                         | Key Examples                                                            |
|------------------------------------|---------------------------------------------------------------------------|-------------------------------------------------------------------------|
| **High-Read, Low-Write Data**      | Data accessed frequently but updated rarely                               | User profiles, configuration settings, product catalogs, reference data |
| **Expensive Computations**         | Results of intensive processing that don\'t need real-time freshness      | Complex queries, ML inferences, API aggregations, report generation     |
| **Performance & Latency Critical** | Where response time directly impacts user experience or system throughput | Real-time applications, mobile apps, global services                    |
| **Scalability Bottlenecks**        | To reduce load on backend systems and enable horizontal scaling           | Database query reduction, API rate limiting, session management         |
| **Geographic Distribution**        | Minimizing latency across different regions                               | CDN for static assets, regional caching for global user bases           |

**When NOT to Use Caching**

| Scenario                              | Reason                                   |
|---------------------------------------|------------------------------------------|
| **Frequently changing data**          | High invalidation overhead, low hit rate |
| **Write-heavy workloads**             | Cache becomes a bottleneck               |
| **Data requiring strong consistency** | Cache coherence too complex              |
| **Trivial data access**               | Not worth the complexity overhead        |
| **Data smaller than cache overhead**  | Negative performance impact              |


**Caching Checklist**

| Criterion                            | Why It Helps                                       |
|--------------------------------------|----------------------------------------------------|
| **Read-heavy, rarely changes**       | Maximizes cache hit rate and ROI                   |
| **Source system is slow/expensive**  | Reduces load and cost on primary systems           |
| **Low latency is critical**          | Improves user experience and system responsiveness |
| **Computation is intensive**         | Avoids redundant processing                        |
| **Geographically distributed users** | Reduces cross-region latency                       |

**Specific Good Candidates**

| Cache Candidate                  | Why It Works Well                                       |
|----------------------------------|---------------------------------------------------------|
| **Reference/Configuration Data** | Rarely changes, accessed frequently                     |
| **User Sessions**                | Short-lived, read-intensive patterns                    |
| **API Responses**                | Reduces backend load, especially for identical requests |
| **Authentication Tokens**        | Fast validation without database hits                   |
| **Rate Limiting Counters**       | Requires fast atomic operations                         |
| **Product Catalogs**             | High read volume, infrequent updates                    |
| **Leaderboard Data**             | Real-time updates with fast reads                       |

# Common Caching Tools

Caching tools are essential for improving application performance by storing frequently accessed data in memory (in-memory caching) or at the HTTP level (web/page caching). The most common ones fall into two main categories:

- **In-memory/object caching** (e.g., for database queries, sessions, API results):
  - Redis
  - Memcached
  - Hazelcast
  - Aerospike
- **Web/HTTP caching** (e.g., for full pages, static assets):
  - Varnish
  - Nginx (with caching modules)
  - Squid

Other notable mentions include Apache Ignite, Couchbase, and CDN-integrated tools like Cloudflare, but the above are the core open-source options widely used in 2025.

**Comparison Table**

| **Tool** | **Type** | **Data Structures Supported** | **Persistence** | **High Availability/Replication** | **Multi-threaded** | **Best For** | **Drawbacks** |
| --- | --- | --- | --- | --- | --- | --- | --- |
| **Redis** | In-memory key-value store | Strings, hashes, lists, sets, sorted sets, bitmaps, hyperloglogs, etc. | Yes (RDB/AOF) | Yes (clustering, sentinel) | No (single-threaded event loop) | Versatile caching, sessions, pub/sub, queues, leaderboards, real-time apps | Single-threaded core limits extreme CPU scaling; persistence adds overhead |
| **Memcached** | Distributed in-memory cache | Simple key-value (strings/objects) | No  | Basic (client-side sharding) | Yes | Simple, high-throughput string/object caching | No persistence, limited data types, no native clustering |
| **Hazelcast** | In-memory data grid | Distributed maps, queues, topics, etc. | Optional | Yes (strong consistency, partitioning) | Yes | Java-centric distributed apps, session clustering | Heavier footprint; primarily for Java ecosystems |
| **Aerospike** | NoSQL in-memory database | Complex types (lists, maps, records) | Yes (hybrid memory+SSD) | Yes (strong consistency, auto-sharding) | Yes | High-scale real-time analytics with persistence | Steeper learning curve; more complex setup |
| **Varnish** | HTTP accelerator | HTTP responses/pages | No (memory only) | Basic (via VCL/load balancing) | Yes | Full-page and edge caching for web content | Configuration via VCL; not for object caching |
| **Nginx** | Web server + reverse proxy | HTTP content, proxy cache | Disk-based optional | Via load balancing | Yes | Reverse proxy caching, static assets serving | Less flexible cache invalidation than Varnish |
| **Squid** | Forward/reverse proxy | HTTP objects | Disk-based | Basic | Yes | Traditional proxy caching in networks | Older technology; less common in modern stacks |

**Ranking by Usage/Popularity (2025)**

Based on adoption trends, developer surveys, market share in cloud services (e.g., AWS ElastiCache), and mentions in benchmarks/articles:

- **Redis** - By far the most popular and widely used. It's versatile, feature-rich, and dominates in modern stacks (e.g., microservices, real-time apps). Often the default choice for new projects.
- **Memcached** - Still common for simple, high-speed caching, especially in legacy systems or where raw throughput matters. Losing ground to Redis but reliable.
- **Varnish/Nginx** - Top for web/HTTP caching; Nginx is ubiquitous as a server/proxy, with caching as a bonus.
- **Hazelcast/Aerospike** - Niche but growing in enterprise/Java or high-consistency scenarios.
- **Squid** - Less popular now, more for traditional proxy setups.

Redis leads due to its balance of speed, features, and ecosystem support. For pure simplicity and speed in basic key-value scenarios, Memcached holds strong. Choose based on needs: complex data → Redis; simple strings → Memcached; full pages → Varnish/Nginx.

# Introduction to Redis

**Overview:**

Redis (REmote DIctionary Server) is an open-source, in-memory data
structure store used as a database, cache, message broker, and more.
Created by Salvatore Sanfilippo in 2009.

 **Key Features**:

- Supports data structures: Strings, Hashes, Lists, Sets, Sorted Sets,
  Bitmaps, HyperLogLogs, Streams.

- Persistence: Optional RDB snapshots or AOF logs for durability.

- High Performance: 100k+ ops/sec, sub-ms latency.

- Replication & Clustering: For high availability and scaling.

- Pub/Sub: For real-time messaging.

- Lua Scripting: Custom atomic operations.

 **Use Cases**: Caching, session stores, queues, leaderboards, real-time analytics.

# Installation on Windows
1. Microsoft's Redis for Windows (Recommended for Production)

    *Install via Windows Package Manager (WinGet)*

    *Install via Chocolatey*

2. Windows Subsystem for Linux (WSL2) - Most Common Approach
3. Docker for Windows
4. Memurai (Commercial Alternative)
5. MSI Installer (Old version - not recommended)


# Installation on Linux
**1. OS commands**
```sh
# Add Redis repository
curl -fsSL https://packages.redis.io/gpg | sudo gpg --dearmor -o /usr/share/keyrings/redis-archive-keyring.gpg

# For Ubuntu 22.04/Jammy:
echo "deb [signed-by=/usr/share/keyrings/redis-archive-keyring.gpg] https://packages.redis.io/deb $(lsb_release -cs) main" | sudo tee /etc/apt/sources.list.d/redis.list

# Update and install
sudo apt update
sudo apt install redis
```

```sh
# Add EPEL repository (for CentOS/RHEL)
sudo dnf install epel-release

# Install Redis
sudo dnf install redis

# Start and enable
sudo systemctl start redis
sudo systemctl enable redis
```

**2. From Source (Latest Version)**
```sh
# Install dependencies
sudo apt install build-essential tcl # Ubuntu/Debian
# OR
sudo dnf groupinstall "Development Tools" # RHEL/CentOS/Fedora

# Download latest stable version
curl -O https://download.redis.io/redis-stable.tar.gz
tar xzf redis-stable.tar.gz
cd redis-stable

# Compile
make

# Test compilation (optional but recommended)
make test

# Install system-wide
sudo make install

# Create configuration directory
sudo mkdir /etc/redis
sudo cp redis.conf /etc/redis/

# Create systemd service (optional)
# Copy systemd service file if available or create one
```

**3.Using Docker**
```sh
# Pull Redis image
docker pull redis

# Run Redis container
docker run --name redis -d -p 6379:6379 redis

# With persistent storage
docker run --name redis -d -p 6379:6379 -v /path/to/data:/data redis redis-server --appendonly yes
```

**4. Using Redis Server Script (Simplified)**
```sh
#!/bin/bash
# Save as install_redis.sh and run: bash install_redis.sh

# For Ubuntu/Debian
if [ -f /etc/debian_version ]; then
    sudo apt update
    sudo apt install -y redis-server
    sudo systemctl enable redis-server
    sudo systemctl start redis-server

# For CentOS/RHEL
elif [ -f /etc/redhat-release ]; then
    sudo yum install -y epel-release
    sudo yum install -y redis
    sudo systemctl enable redis
    sudo systemctl start redis
fi

# Test installation
redis-cli ping
```

## Docker Compose on both Systems
**generate required compose file**
```yml
version: '3.8'

services:
  redis:
    image: redis:7-alpine
    container_name: redis
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    restart: unless-stopped
    command: redis-server --appendonly yes

volumes:
  redis_data:
```
**Run Redis**
```sh
# Start Redis
docker-compose up -d

# View logs
docker-compose logs -f redis

# Stop Redis
docker-compose down

# Stop and remove volumes
docker-compose down -v

# Check status
docker-compose ps
```

**Connect to Redis CLI**
```sh
# Direct connection
docker-compose exec redis redis-cli

# With authentication
docker-compose exec redis redis-cli -a yourpassword

# From host machine
redis-cli -h localhost -p 6379
```
**Environment Variables File (.env)**
```env
REDIS_PASSWORD=YourSecurePassword123
REDIS_PORT=6379
```
