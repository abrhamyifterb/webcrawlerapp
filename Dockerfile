FROM node:14-alpine

WORKDIR /frontend

COPY frontend/package.json ./

RUN npm install --save

COPY . .

EXPOSE 3000

CMD ["npm", "start"]
