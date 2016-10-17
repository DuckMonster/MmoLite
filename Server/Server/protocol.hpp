#pragma once
namespace protocol {
	enum MmoProtocol {
		Ping,
		PlayerID,

		ActorJoin,
		ActorLeave,

		ActorPosition,
		ActorRotation,

		PlayerPossess,
		PlayerInput
	};
}