#pragma once
namespace protocol {
	enum MmoProtocol {
		Ping,

		PlayerID,
		PlayerJoin,
		PlayerLeave,
		PlayerPossess,

		PlayerPosition,
		PlayerRotation,
		PlayerInput
	};
}