import React, { useState, useEffect, useCallback } from "react";
import { checkUserAuth } from "../Services/authServices";

let logoutTimer;

const AuthContext = React.createContext({
  token: "",
  role: "",
  userType: "",
  isService: null,
  username: "",
  uid: "",
  isLoggedIn: false,
  login: (token) => {},
  logout: () => {},
  checkAuth: () => {},
});

const calculateRemainingTime = (expirationTime) => {
  const currentTime = new Date().getTime();
  const adjExpirationTime = new Date(expirationTime).getTime();

  const remainingDuration = adjExpirationTime - currentTime;

  return remainingDuration;
};

const retrieveStoredToken = () => {
  const storedToken = localStorage.getItem("token");
  const storedExpirationDate = localStorage.getItem("expirationTime");
  const storedRole = localStorage.getItem("role");
  const storedUserType = localStorage.getItem("userType");
  const storedIsService = localStorage.getItem("isService");
  const storedUid = localStorage.getItem("uid");
  const storedUserName = localStorage.getItem("username");

  const remainingTime = calculateRemainingTime(storedExpirationDate);

  if (remainingTime <= 3600) {
    localStorage.removeItem("token");
    localStorage.removeItem("expirationTime");
    localStorage.removeItem("role");
    localStorage.removeItem("userType");
    localStorage.removeItem("isService");
    localStorage.removeItem("uid");
    localStorage.removeItem("username");
    localStorage.clear();
    return null;
  }

  return {
    token: storedToken,
    duration: remainingTime,
    role: storedRole,
    userType: storedUserType,
    isService: storedIsService === "true" ? true : false,
    uid: storedUid,
    username: storedUserName,
    isLoggedIn: false,
  };
};

export const AuthContextProvider = (props) => {
  const tokenData = retrieveStoredToken();

  let initialToken;
  let initialUserType;
  let initialUserName;
  let initialIsService;
  if (tokenData) {
    initialToken = tokenData.token;
    initialUserType = tokenData.userType;
    initialUserName = tokenData.username;
    initialIsService = tokenData.isService;
  }

  const [token, setToken] = useState(initialToken);
  const [userType, setUserType] = useState(initialUserType);
  const [username, setUserName] = useState(initialUserName);
  const [isService, setIsService] = useState(initialIsService);

  const userIsLoggedIn = !!token;

  const logoutHandler = useCallback(() => {
    setToken(null);
    localStorage.removeItem("token");
    localStorage.removeItem("expirationTime");
    localStorage.removeItem("role");
    localStorage.removeItem("userType");
    localStorage.removeItem("isService");
    localStorage.removeItem("uid");
    localStorage.removeItem("username");
    localStorage.clear();

    if (logoutTimer) {
      clearTimeout(logoutTimer);
    }
  }, []);

  const loginHandler = (
    token,
    userType,
    username,
    isService,
    expirationTime
  ) => {
    setToken(token);
    setUserType(userType);
    setUserName(username);
    setIsService(isService);
    localStorage.setItem("token", token);
    localStorage.setItem("userType", userType);
    localStorage.setItem("username", username);
    localStorage.setItem("isService", isService);
    localStorage.setItem("expirationTime", expirationTime);

    const remainingTime = calculateRemainingTime(expirationTime);

    logoutTimer = setTimeout(logoutHandler, remainingTime);
  };

  function checkAuthHandler() {
    checkUserAuth()
      .then((objRespone) => {
        if (objRespone.success === true) {
          const remainingTime = calculateRemainingTime(
            new Date(new Date().getTime() + +3600 * 1000)
          );
          logoutTimer = setTimeout(logoutHandler, remainingTime);
        }
      })
      .catch((err) => {
        if (err === 401 || err === 405) {
          // sai token
          logoutHandler();
        }
      });
  }

  useEffect(() => {
    if (tokenData) {
      // console.log(tokenData.duration);
      logoutTimer = setTimeout(logoutHandler, tokenData.duration);
    }
  }, [tokenData, logoutHandler]);

  const contextValue = {
    token: token,
    isLoggedIn: userIsLoggedIn,
    userType: userType,
    username: username,
    isService: isService,
    login: loginHandler,
    logout: logoutHandler,
    checkAuth: checkAuthHandler,
  };

  return (
    <AuthContext.Provider value={contextValue}>
      {props.children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
