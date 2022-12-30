# pull official base image
FROM node:18.12.1-alpine

# set working directory
WORKDIR /app

# add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

# install app dependencies
COPY ./SystemMonitor.WebApp/package-lock.json ./
COPY ./SystemMonitor.WebApp/package.json ./
RUN npm install --silent

# add app
COPY ./SystemMonitor.WebApp/ ./

# start app
CMD ["npm", "start"]
